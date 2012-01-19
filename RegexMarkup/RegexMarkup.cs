using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using RegexMarkup.Properties;
using RegexMarkup.Forms;

namespace RegexMarkup
{
    public sealed class RegexMarkup
    {
        #region Singleton Implement
        /// <summary>
        /// C�digo para llamar a la clase como un singleton
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
        private Waiting waitForm = null;
        #region startMarkup
        ///<summary>
        ///Procedimiento al que llamaremos para inciar el proceso de marcaci�n
        ///</summary>
        public void startMarkup(Office.CommandBarButton ctrl, ref bool cancel)
        {
            //DTDStruct DTDarticle = new DTDStruct();
            /* Declaracion de variables */
            Boolean marked = true;
            String patternString = null;
            String subjetcString = null;
            String markedString = null;
            String replaceText = null;
            String issn = null;
            String pathXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "regex.xml");
            Word.Selection docSeleccion = null;
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode objElem = null;
            XmlNode structNode = null;
            List<markupStruct> citas = null;

            /* Inicializamos variables */
            ActiveDocument = Globals.ThisAddIn.Application.ActiveDocument;
            /* Leemos y verificamos que el iss exista */
            issn = getAttrValueInTag("article", "issn");
            if (issn == null) {
                MessageBox.Show(Resources.RegexMarkup_issnNotDefined, Resources.RegexMarkup_title);
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
                    MessageBox.Show(Resources.RegexMarkup_serilaNotInXML, Resources.RegexMarkup_title);
                }else { 
                    /* Asignamos los struct en los que se divide la revista */
                    structNode = objElem.SelectSingleNode("struct");
                    /* Patr�n de b�squeda */
                    
                    patternString = objElem.SelectSingleNode("regex").InnerText;
                    //MessageBox.Show("Pattern String");
                    /* Verificamos que la seleccion sea del parrafo completo */
                    Word.Range start = Globals.ThisAddIn.Application.Selection.Range;
                    docSeleccion = Globals.ThisAddIn.Application.Selection;
                    if (docSeleccion.Start != docSeleccion.Paragraphs.First.Range.Start || docSeleccion.End != docSeleccion.Paragraphs.Last.Range.End){
                        MessageBox.Show(Resources.RegexMarkup_selectCitationComplete, Resources.RegexMarkup_title);
                    } else {
                        /* Asignamos el texto de la seleccion a subjectString */
                        subjetcString = docSeleccion.Range.Text;
                        //MessageBox.Show(subjetcString, "Texto Seleccionado");
                        /* Inicializando arratlist de citas */
                        citas = new List<markupStruct>();
                        /* Buscando parrafo por parrafo */
                        foreach (Word.Paragraph parrafo in docSeleccion.Paragraphs){
                            /* Mandamos el texto de cada parrafo a una funcion que nos lo regresara marcado y quitamos el salto linea */
                            object parrafoStart = parrafo.Range.Start;
                            object parrafoEnd = (parrafo.Range.End - 1);
                            subjetcString = ActiveDocument.Range(ref parrafoStart, ref parrafoEnd).Text;
                            //MessageBox.Show(subjetcString, "Texto de parrafo");
                            if (subjetcString == null)
                            {
                                citas[citas.Count - 1].BreakLines += "\r";
                            }
                            else
                            {
                                try
                                {
                                    markedString = this.markupText(patternString, subjetcString, structNode);
                                }
                                catch (Exception e) {
                                    MessageBox.Show(e.Message);
                                }
                                marked = true;
                                if (subjetcString == markedString)
                                {
                                    marked = false;
                                }
                                citas.Add(new markupStruct(subjetcString , markedString , marked));
                            }
                        }
                        ValidateMarkup formValidate = new ValidateMarkup(ref citas, ref structNode);
                        formValidate.ShowDialog();
                        /* Reemplzando texto original por el marcado */
                        foreach (markupStruct cita in citas) {
                            if(cita.Marked){
                                replaceText += cita.MarkedStr + cita.BreakLines;
                            }else{
                                replaceText += cita.OriginalStr + cita.BreakLines;
                            }
                             
                        }
                        docSeleccion.Range.Text = replaceText;
                        
                        /* Volvemos a seleccionar el texto desde el inicio de la seleccion inicial mas el total de la cadena con etiquetas */
                        int selectionStart = docSeleccion.Range.Start;
                        int selectionEnd = (docSeleccion.Range.Start + replaceText.Length);
                        start.SetRange(selectionStart, selectionEnd);
                        start.Select();
                        /* Coloreando Etiquetas (Tags) */
                        Globals.ThisAddIn.Application.Visible = false;
                        waitForm = Waiting.Instance;
                        waitForm.Show();
                        foreach (Word.Paragraph parrafo in docSeleccion.Paragraphs)
                        {
                            Word.Range refrange = parrafo.Range;
                            this.colorRefTags(ref refrange, structNode, 0);
                        }
                        waitForm.Hide();
                        Globals.ThisAddIn.Application.Visible = true;
                    }

                }

            }
        }
        #endregion

        #region getAttrValueInTag
        /// <summary>
        /// Funci�n para obtener el atributo de una etiqueta(TAG)
        /// </summary>
        /// <param name="tag">Etiqueta donde buscaremos</param>
        /// <param name="attr">Atributo a buscar</param>
        /// <returns>El valor del atributo buscado</returns>
        private String getAttrValueInTag(String tag, String attr) {
            /* Declaraci�n de variables */
            String subjectString = null;
            String result = null;
            String pattern = null;
            Regex regexObj = null;
            Match matchResults = null;
            RegexOptions options = RegexOptions.IgnoreCase;
            /* Inicializamos variables */
            pattern = "\\[" + tag + ".*?" + attr + "=(\\\".*?\\\"|\\'.*?\\'|[^\\ \\]]*).*?\\]";
            subjectString = ActiveDocument.Content.Text;
            /* Verificamos que haya coincidencia */
            regexObj = new Regex(pattern, options);
            matchResults = regexObj.Match(subjectString);
            if (matchResults.Success)
            {
                result = matchResults.Groups[1].Value;
                if (result.EndsWith("\"") || result.EndsWith("'"))
                {
                    result = result.Substring(1, result.Length - 2);
                }
            }
            else
            {
                return result;
            }
            return result;
        }
        #endregion
   
        #region markupText
        /// <summary>
        /// Funci�n para marcar la cadena de texto
        /// </summary>
        /// <param name="refPattern">Expresi�n regular</param>
        /// <param name="refString">Cadena para buscar coincidencias</param>
        /// <param name="refStruct">Instrucciones de marcado</param>
        /// <returns>La cadena marcada</returns>
        private String markupText(String refPattern, String refString, XmlNode refStruct) {
            /* Definici�n de variables */
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
            XmlNode structNode = null;
            XmlNodeList multipleOptions = null;
            XmlNode singleOptionStruct = null;
            /* Verificamos si la cadena es nula antes de evaluarla */
            if (refString != null)
            {
                /* Iniciando b�squeda del patron en la cadena de texto */
                objRegExp = new Regex(refPattern, options);
                matchResults = objRegExp.Match(refString);
                /* Verificamos si hay alguna coincidencia e iteramos todas las coincidencias encontradas*/
                if (matchResults.Success)
                {
                    while (matchResults.Success)
                    {
                        /* Iteramos los nodos dentro del xml que nos dan el contenido de las citas */
                        foreach (XmlNode itemXML in refStruct.ChildNodes)
                        {
                            /* Verificamos si el nodo es una etiqueta(tag) o no */
                            if (itemXML.Attributes.GetNamedItem("tag") != null)
                            {
                                /*Verificamos y agregamos atributos por omisi�n  a la etiqueta de apertura*/
                                if (itemXML.SelectSingleNode("attr") == null)
                                {
                                    tagStringOpen = "[" + itemXML.Name + "]";
                                }
                                else {
                                    tagStringOpen = "[" + itemXML.Name;
                                    foreach (XmlNode attrNode in itemXML.SelectSingleNode("attr"))
                                    {
                                        if (attrNode.Name == "dateiso" && attrNode.InnerText == "")
                                        {
                                            tagStringOpen += " " + attrNode.Name + "=\"" + objRegExp.Replace(refString, itemXML.SelectSingleNode("value").InnerText) +"0000\"";
                                        }
                                        else
                                        {
                                            tagStringOpen += " " + attrNode.Name + "=\"" + attrNode.InnerText + "\"";
                                        }
                                    }
                                    tagStringOpen += "]";
                                }
                                tagStringClose = "[/" + itemXML.Name + "]";
                            }
                            else
                            {
                                tagStringOpen = null;
                                tagStringClose = null;
                            }
                            /* Verificamos si el nodo contiene un valor directo para la etiqueta(tag) o si su valor esta compuesto otras etiquetas(tag) */
                            if (itemXML.SelectSingleNode("value") == null)
                            {
                                /* Si esta compuesto de otras etiquetas(tag) volvemos a enviar la cadena y el patron con los nodos hijos de la etiqueta(tag) */
                                resultString += tagStringOpen + this.markupText(refPattern, refString, itemXML) + tagStringClose;
                            }
                            else
                            {
                                /* Deacuerdo al valor que tenemos indicado en el xml extramos la cadena de texto correspondiente */
                                backreference = itemXML.SelectSingleNode("value").InnerText;
                                subjectString = objRegExp.Replace(matchResults.Value, backreference);
                                /* Verificamos si a la cadena de texto resultante tiene multiples opciones, hay  que aplicarle un patron nuevo � si agregamos las etiquetas(tag) directamente */
                                if (itemXML.SelectSingleNode("multiple") != null)
                                {
                                    /* Inicializamos la variables locales*/
                                    multipleOptions = itemXML.SelectSingleNode("multiple").ChildNodes;
                                    multipleOptionPattern = null;
                                    singleOptionMatch = false;
                                    /* Armamos la expresion regular para todas las opciones y asignamos un namedgroup a cada una */
                                    for (int i = 0; i < multipleOptions.Count; i++)
                                    {
                                        singleOptionPattern = multipleOptions[i].SelectSingleNode("regex").InnerText;
                                        singleOptionStruct = multipleOptions[i].SelectSingleNode("struct");
                                        if (i == 0)
                                        {
                                            multipleOptionPattern += "(?<op" + i + ">" + singleOptionPattern + ")";
                                        }
                                        else
                                        {
                                            multipleOptionPattern += "|(?<op" + i + ">" + singleOptionPattern + ")";
                                        }
                                    }
                                    /* Inicializamos un nuevo metro para evaluar la expresion regular de todas la opciones */
                                    multipleOptionRegExp = new Regex(multipleOptionPattern, options);
                                    multipleOptionsMarchResults = multipleOptionRegExp.Match(subjectString);

                                    /* Verificamos que opcion es la que coincidio*/
                                    for (int i = 0; i < multipleOptions.Count && !singleOptionMatch; i++)
                                    {
                                        if (multipleOptionsMarchResults.Groups["op" + i].Success)
                                        {
                                            singleOptionMatched = i;
                                            singleOptionMatch = true;
                                        }
                                    }

                                    singleOptionPattern = multipleOptions[singleOptionMatched].SelectSingleNode("regex").InnerText;
                                    singleOptionStruct = multipleOptions[singleOptionMatched].SelectSingleNode("struct");
                                    /* Verificamos si hay que poner un valor antes de la etiqueta(tag) */
                                    if (itemXML.SelectSingleNode("prevalue") != null)
                                    {
                                        backreferencePreValue = itemXML.SelectSingleNode("prevalue").InnerText;
                                        backreferencePreValueString = objRegExp.Replace(matchResults.Value, backreferencePreValue);
                                        resultString += backreferencePreValueString;
                                    }
                                    /* Enviamos la expresion regular de la opcion que coincidio junto con el grupo de ordenamiento */
                                    resultString += tagStringOpen + this.markupText(singleOptionPattern, subjectString, singleOptionStruct) + tagStringClose;

                                    /* Verificamos si hay que poner un valor despues de la etiqueta(tag) */
                                    if (itemXML.SelectSingleNode("postvalue") != null)
                                    {
                                        backreferencePostValue = itemXML.SelectSingleNode("postvalue").InnerText;
                                        backreferencePostValueString = objRegExp.Replace(matchResults.Value, backreferencePostValue);
                                        resultString += backreferencePostValueString;
                                    }

                                }
                                else if (itemXML.SelectSingleNode("regex") != null)
                                {
                                    /* Verificamos si hay que poner un valor antes de la etiqueta(tag) */
                                    if (itemXML.SelectSingleNode("prevalue") != null)
                                    {
                                        backreferencePreValue = itemXML.SelectSingleNode("prevalue").InnerText;
                                        backreferencePreValueString = objRegExp.Replace(matchResults.Value, backreferencePreValue);
                                        resultString += backreferencePreValueString;
                                    }
                                    structNode = itemXML.SelectSingleNode("struct");
                                    patternString = itemXML.SelectSingleNode("regex").InnerText;
                                    /* Enviamos la cadena resultante con el patron nuevo a aplicar y las etiquetas(tag) que debe contener y el resultado lo ponemos dentro de la etiqueta(tag) correspondiente */
                                    resultString += tagStringOpen + this.markupText(patternString, subjectString, structNode) + tagStringClose;
                                    if (itemXML.SelectSingleNode("postvalue") != null)
                                    {
                                        backreferencePostValue = itemXML.SelectSingleNode("postvalue").InnerText;
                                        backreferencePostValueString = objRegExp.Replace(matchResults.Value, backreferencePostValue);
                                        resultString += backreferencePostValueString;
                                    }
                                }
                                else
                                {
                                    replaceString = null;
                                    /* Verificamos si hay que poner un valor antes de la etiqueta(tag) */
                                    if (itemXML.SelectSingleNode("prevalue") != null)
                                    {
                                        backreferencePreValue = itemXML.SelectSingleNode("prevalue").InnerText;
                                        replaceString += backreferencePreValue;
                                    }
                                    /* Armamos la etiqueta(tag) con su valor */
                                    replaceString = replaceString + tagStringOpen + backreference + tagStringClose;
                                    if (itemXML.SelectSingleNode("postvalue") != null)
                                    {
                                        backreferencePostValue = itemXML.SelectSingleNode("postvalue").InnerText;
                                        replaceString += backreferencePostValue;
                                    }
                                    resultString += objRegExp.Replace(refString, replaceString);
                                }
                            }
                        }
                        matchResults = matchResults.NextMatch();
                    }
                }
                else
                {
                    resultString += refString;
                }
            } else {
                resultString +=  refString;
            }
            return resultString;
        }
        #endregion

        #region colorTags
        /// <summary>
        /// Function to colorize tags
        /// </summary>
        /// <param name="colorizeRange">Rango donde se coloreara el texto</param>
        /// <param name="structNode">Contenido de la etiqueta actual con sus hijos</param>
        /// <param name="color">Color de la etiqueta {0,...,4}</param>
        public void colorRefTags(ref Word.Range colorizeRange, XmlNode structNode, int color) {
            /* Definimos y asignamos el arreglo de colores para las etiquetas */
            Microsoft.Office.Interop.Word.WdColor[] colors = new Microsoft.Office.Interop.Word.WdColor[]{
                Word.WdColor.wdColorDarkBlue,
                Word.WdColor.wdColorTeal,
                Word.WdColor.wdColorGray50,
                Word.WdColor.wdColorBlue,
                Word.WdColor.wdColorViolet
            };
            object missingval = System.Type.Missing;
            object replaceAll = Word.WdReplace.wdReplaceAll;
            /* Si la estructura enviada contiene una etiqueta aumentamos el numero de color para las etiquetas hijas */
            if (structNode.Attributes.GetNamedItem("tag") != null) {
                color++;
                /* Si el indice de color llego a 20 lo reiniciamos a 0 */
                color = color == 5 ? 0 : color;
            }
            /* Iteramos las etiquetas(tags) hijas*/
            foreach (XmlNode tag in structNode.ChildNodes){
                try
                {   
                    /* Al igual que en marcado analizamos la condiciones existentes en donde se tienen que marcar etiquetas(tags) hijas */
                    if (tag.SelectSingleNode("value") == null && tag.ChildNodes.Count > 0)
                    {
                        this.colorRefTags(ref colorizeRange, tag, color);
                    }
                    else
                    {
                        if (tag.SelectSingleNode("multiple") != null && tag.ChildNodes.Count > 0)
                        {
                            this.colorRefTags(ref colorizeRange, tag, color);
                        }
                        else if (tag.SelectSingleNode("regex") != null && tag.ChildNodes.Count > 0)
                        {
                            this.colorRefTags(ref colorizeRange, tag, color);
                        }
                    }
                    /* Si el atributo indica que es una etiqueta(tag) la coloreamos */
                    if (tag.Attributes != null &&tag.Attributes.GetNamedItem("tag") != null)
                    {
                        /* Buscamos y coloreamos el inicio de la etiqueta(tag) */
                        colorizeRange.Find.ClearFormatting();
                        colorizeRange.Find.Text = "\\[" + tag.Name + "*\\]";
                        colorizeRange.Find.MatchWildcards = true;
                        colorizeRange.Find.Replacement.ClearFormatting();
                        colorizeRange.Find.Replacement.Font.Color = colors[color];
                        colorizeRange.Find.Replacement.Font.Size = 11;
                        colorizeRange.Find.Replacement.Font.Name = "Arial";
                        colorizeRange.Find.Execute(ref missingval, ref missingval, ref missingval,
                            ref missingval, ref missingval, ref missingval, ref missingval,
                            ref missingval, ref missingval, ref missingval, ref replaceAll,
                            ref missingval, ref missingval, ref missingval, ref missingval);
                        /* Buscamos y coloreamos el cierre de la etiqueta(tag) */
                        colorizeRange.Find.ClearFormatting();
                        colorizeRange.Find.Text = "\\[/" + tag.Name + "\\]";
                        colorizeRange.Find.Replacement.ClearFormatting();
                        colorizeRange.Find.Replacement.Font.Color = colors[color];
                        colorizeRange.Find.Replacement.Font.Size = 11;
                        colorizeRange.Find.Replacement.Font.Name = "Arial";
                        colorizeRange.Find.Execute(ref missingval, ref missingval, ref missingval,
                            ref missingval, ref missingval, ref missingval, ref missingval,
                            ref missingval, ref missingval, ref missingval, ref replaceAll,
                            ref missingval, ref missingval, ref missingval, ref missingval);
                    }
                }catch(Exception e){
                    MessageBox.Show(e.Message);
                }
            }
        }
        #endregion

    }
}
