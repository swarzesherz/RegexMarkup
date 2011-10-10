using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;

namespace RegexMarkup
{
    public sealed class RegexMarkup
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static RegexMarkup instance=null;
        static readonly object padlock = new object();

        RegexMarkup()
        {
        }

        public static RegexMarkup Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new RegexMarkup();
                    }
                    return instance;
                }
            }
        }
        #endregion

        public static Word.Document ActiveDocument = null;
        #region startMarkup
        ///<summary>
        ///Procedimiento al que llamaremos para inciar el proceso de marcación
        ///</summary>
        public void startMarkup(Office.CommandBarButton ctrl, ref bool cancel)
        {
            /* Declaracion de variables */
            String patternString = null;
            String subjetcString = null;
            String replaceText = null;
            String issn = null;
            String pathXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "regex.xml");
            Word.Selection docSeleccion = null;
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode objElem = null;
            XmlNode groupsXML = null;
            /* Inicializamos variables */
            ActiveDocument = Globals.ThisAddIn.Application.ActiveDocument;
            /* Leemos y verificamos que el iss exista */
            issn = getAttrValueInTag("article", "issn");
            if (issn == null) {
                MessageBox.Show("No esta definido issn", "RegexMarkup");
            } else {
                //MessageBox.Show(issn, "ISSN");
                /* Cargamos el archivo xml donde se encuetran los patrones de las revistas */
                try
                {
                    xmlDoc.Load(pathXML);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }
                /* Leemos el nodo correspondiente al issn de la revista */
                objElem = xmlDoc.SelectSingleNode("//*[@issn=\"" + issn + "\"]");
                if (objElem == null){
                    MessageBox.Show("No se econtro la revista en el archivo xml", "RegexMarkup");
                }else { 
                    /* Asignamos los grupos en los que se divide la revista */
                    groupsXML = objElem.SelectSingleNode("grupos");
                    /* Patrón de búsqueda */
                    
                    patternString = objElem.SelectSingleNode("regex").InnerText;
                    //MessageBox.Show("Pattern String");
                    /* Verificamos que la seleccion sea del parrafo completo */
                    docSeleccion = Globals.ThisAddIn.Application.Selection;
                    if (docSeleccion.Start != docSeleccion.Paragraphs.First.Range.Start || docSeleccion.End != docSeleccion.Paragraphs.Last.Range.End){
                        MessageBox.Show("Seleccione la cita(s) completamente");
                    } else {
                        /* Asignamos el texto de la seleccion a subjectString */
                        subjetcString = docSeleccion.Range.Text;
                        //MessageBox.Show(subjetcString, "Texto Seleccionado");
                        /* Buscando parrafo por parrafo */
                        foreach (Word.Paragraph parrafo in docSeleccion.Paragraphs){
                            /* Mandamos el texto de cada parrafo a una funcion que nos lo regresara marcado y quitamos el salto linea */
                            object parrafoStart = parrafo.Range.Start;
                            object parrafoEnd = (parrafo.Range.End - 1);
                            subjetcString = ActiveDocument.Range(ref parrafoStart, ref parrafoEnd).Text;
                            //MessageBox.Show(subjetcString, "Texto de parrafo");
                            replaceText = replaceText + markupText(patternString, subjetcString, groupsXML) + "\r";
                        }
                        docSeleccion.Range.Text = replaceText;
                    }

                }

            }
        }
        #endregion

        #region getAttrValueInTag
        /// <summary
        /// Función para obtener el atributo de una etiqueta(TAG)
        /// </sumary>
        private String getAttrValueInTag(String tag, String attr) {
            /* Declaración de variables */
            String subjectString = null;
            String result = null;
            String pattern = null;
            Regex regexObj = null;
            Match matchResults = null;
            RegexOptions options = RegexOptions.IgnoreCase;
            /* Inicializamos variables */
            pattern = "\\[" + tag + ".*?" + attr + "=\"(.*?)\".*?\\]";
            subjectString = ActiveDocument.Content.Text;
            /* Verificamos que haya coincidencia */
            try
            {
                regexObj = new Regex(pattern, options);
                matchResults = regexObj.Match(subjectString);
                if (matchResults.Success)
                {
                    result = matchResults.Groups[1].Value;
                }
                else
                {
                    return result;
                }
            }
            catch (ArgumentException e)
            {
                // Syntax error in the regular expression
            }
            return result;
        }
        #endregion
   
        #region markupTex
        /// <summary
        /// Función para marcar la cadena de texto
        /// </sumary>
        private String markupText(String refPattern, String refString, XmlNode refGroups) {
            /* Definición de variables */
            bool singleOptionMatch = false;
            int  singleOptionMatched = 0;
            String patternString = null;
            String subjectString = null;
            String tagStringOpen = null;
            String tagStringClose = null;
            String backreference = null;
            String backreferencePostValue = null;
            String backreferencePostValueString = null;
            String backreferencePreValue = null;
            String backreferencePreValueString = null;
            String replaceString = null;
            String resultString = null;
            String multipleOptionPattern = null;
            String singleOptionPattern = null;
            Regex objRegExp = null;
            Regex multipleOptionRegExp = null;
            RegexOptions options = RegexOptions.IgnoreCase;
            Match matchResults = null;
            Match multipleOptionsMarchResults = null;
            XmlNode groupsXML = null;
            XmlNodeList multipleOptions = null;
            XmlNode singleOptionGroups = null;
            /* Iniciando búsqueda del patron en la cadena de texto */
            objRegExp = new Regex(refPattern, options);
            matchResults = objRegExp.Match(refString);
            /* Verificamos si hay alguna coincidencia e iteramos todas las coincidencias encontradas*/
            if (matchResults.Success){
                while (matchResults.Success) {
                    /* Iteramos los nodos dentro del xml que nos dan el contenido de las citas */
                    foreach (XmlNode itemXML in refGroups.ChildNodes){
                        /* Verificamos si el nodo es una etiqueta(tag) o no */
                        if (itemXML.Attributes.GetNamedItem("notag") == null)
                        {
                            tagStringOpen = "[" + itemXML.Name + "]";
                            tagStringClose = "[/" + itemXML.Name + "]";
                        } else {
                            tagStringOpen = null;
                            tagStringClose = null;
                        }
                        /* Verificamos si el nodo contiene un valor directo para la etiqueta(tag) o si su valor esta compuesto otras etiquetas(tag) */
                        if (itemXML.SelectSingleNode("value") == null) {
                            /* Si esta compuesto de otras etiquetas(tag) volvemos a enviar la cadena y el patron con los nodos hijos de la etiqueta(tag) */
                            resultString = resultString + tagStringOpen + markupText(refPattern, refString, itemXML) + tagStringClose;
                        } else {
                            /* Deacuerdo al valor que tenemos indicado en el xml extramos la cadena de texto correspondiente */
                            backreference = itemXML.SelectSingleNode("value").InnerText;
                            subjectString = objRegExp.Replace(matchResults.Value, backreference);
                            /* Verificamos si a la cadena de texto resultante tiene multiples opciones, hay  que aplicarle un patron nuevo ó si agregamos las etiquetas(tag) directamente */
                            if(itemXML.SelectSingleNode("multiple") != null){
                                /* Inicializamos la variables locales*/
                                multipleOptions = itemXML.SelectSingleNode("multiple").ChildNodes;
                                multipleOptionPattern = null;
                                singleOptionMatch = false;
                                /* Armamos la expresion regular para todas las opciones y asignamos un namedgroup a cada una */
                                for (int i = 0; i < multipleOptions.Count; i++)
                                {
                                    singleOptionPattern = multipleOptions[i].SelectSingleNode("regex").InnerText;
                                    singleOptionGroups = multipleOptions[i].SelectSingleNode("grupos");
                                    if (i == 0) {
                                        multipleOptionPattern = multipleOptionPattern + "(?<op" + i + ">" + singleOptionPattern + ")";
                                    } else {
                                        multipleOptionPattern = multipleOptionPattern + "|(?<op" + i + ">" + singleOptionPattern + ")";
                                    }
                                }
                                /* Inicializamos un nuevo metro para evaluar la expresion regular de todas la opciones */
                                multipleOptionRegExp = new Regex(multipleOptionPattern, options);
                                multipleOptionsMarchResults = multipleOptionRegExp.Match(subjectString);

                                /* Verificamos que opcion es la que coincidio*/
                                for (int i = 0; i < multipleOptions.Count && !singleOptionMatch; i++) {
                                    if(multipleOptionsMarchResults.Groups["op" + i].Success){
                                        singleOptionMatched = i;
                                        singleOptionMatch = true;
                                    }
                                }

                                singleOptionPattern = multipleOptions[singleOptionMatched].SelectSingleNode("regex").InnerText;
                                singleOptionGroups = multipleOptions[singleOptionMatched].SelectSingleNode("grupos");
                                /* Verificamos si hay que poner un valor antes de la etiqueta(tag) */
                                if (itemXML.SelectSingleNode("prevalue") != null)
                                {
                                    backreferencePreValue = itemXML.SelectSingleNode("prevalue").InnerText;
                                    backreferencePreValueString = objRegExp.Replace(matchResults.Value, backreferencePreValue);
                                    resultString = resultString + backreferencePreValueString;
                                }
                                /* Enviamos la expresion regular de la opcion que coincidio junto con el grupo de ordenamiento */
                                resultString = resultString + tagStringOpen + markupText(singleOptionPattern, subjectString, singleOptionGroups) + tagStringClose;

                                /* Verificamos si hay que poner un valor despues de la etiqueta(tag) */
                                if (itemXML.SelectSingleNode("postvalue") != null)
                                {
                                    backreferencePostValue = itemXML.SelectSingleNode("postvalue").InnerText;
                                    backreferencePostValueString = objRegExp.Replace(matchResults.Value, backreferencePostValue);
                                    resultString = resultString + backreferencePostValueString;
                                }

                            }
                            else if (itemXML.SelectSingleNode("regex") != null) {
                                /* Verificamos si hay que poner un valor antes de la etiqueta(tag) */
                                if (itemXML.SelectSingleNode("prevalue") != null) {
                                    backreferencePreValue = itemXML.SelectSingleNode("prevalue").InnerText;
                                    backreferencePreValueString = objRegExp.Replace(matchResults.Value, backreferencePreValue);
                                    resultString = resultString + backreferencePreValueString;
                                }
                                groupsXML = itemXML.SelectSingleNode("grupos");
                                patternString = itemXML.SelectSingleNode("regex").InnerText;
                                /* Enviamos la cadena resultante con el patron nuevo a aplicar y las etiquetas(tag) que debe contener y el resultado lo ponemos dentro de la etiqueta(tag) correspondiente */
                                resultString = resultString + tagStringOpen + markupText(patternString, subjectString, groupsXML) + tagStringClose;
                                if (itemXML.SelectSingleNode("postvalue") != null) {
                                    backreferencePostValue = itemXML.SelectSingleNode("postvalue").InnerText;
                                    backreferencePostValueString = objRegExp.Replace(matchResults.Value, backreferencePostValue);
                                    resultString = resultString + backreferencePostValueString;
                                }
                            } else {
                                replaceString = null;
                                /* Verificamos si hay que poner un valor antes de la etiqueta(tag) */
                                if (itemXML.SelectSingleNode("prevalue") != null) {
                                    backreferencePreValue = itemXML.SelectSingleNode("prevalue").InnerText;
                                    replaceString = replaceString + backreferencePreValue;
                                }
                                /* Armamos la etiqueta(tag) con su valor */
                                replaceString = replaceString + tagStringOpen + backreference + tagStringClose;
                                if (itemXML.SelectSingleNode("postvalue") != null) {
                                    backreferencePostValue = itemXML.SelectSingleNode("postvalue").InnerText;
                                    replaceString = replaceString + backreferencePostValue;
                                }
                                resultString = resultString + objRegExp.Replace(refString, replaceString);
                            }
                        }
                    }
                    matchResults = matchResults.NextMatch();
                }
            } else {
                resultString = resultString + refString;
            }
            return resultString;
        }
        #endregion
    }
}
