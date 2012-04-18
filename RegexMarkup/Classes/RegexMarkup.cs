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
using System.Deployment.Application;
using log4net;
using log4net.Config;

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
        private Waiting waitForm = null;
        public static Object missing = Type.Missing;
        private String citationStyle = "other";
        private String dtdVersion = null;
        private String dtdType = "article";
        private ValidateMarkup formValidate = null;
        private Tags tags = Tags.Instance;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region startMarkup
        ///<summary>
        ///Procedimiento al que llamaremos para inciar el proceso de marcación
        ///</summary>
        public void startMarkup()
        {
            if (log.IsInfoEnabled) log.Info("Begin");
            /* Declaracion de variables */
            Boolean marked = true;
            String patternString = null;
            String subjetcString = null;
            String markedString = null;
            String fixedMarkedString = null;
            String issn = null;
            String pathXML = null;
            String clearTag = null;
            Word.Selection docSeleccion = null;
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode objElem = null;
            XmlNode structNode = null;
            List<MarkupStruct> citas = null;
            Regex objRegExp = null;
            RegexOptions options = RegexOptions.None;
            Match matchResults = null;
            Match matchResults2 = null;
            objRegExp = new Regex(@"\[[^\]]+?\]", options);
            /*Definiendo directorios de los xml*/
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                pathXML = Path.Combine(ApplicationDeployment.CurrentDeployment.DataDirectory, "regex.xml");
            }
            else {
                pathXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "regex.xml");
            }
            /*Verificamos si vamos a usar un archivo externo para las reglas*/
            if (Settings.Default.useExternalRegexFile) {
                pathXML = Settings.Default.externalRegexFile;
            }
            /* Inicializamos variables */
            ActiveDocument = Globals.ThisAddIn.Application.ActiveDocument;
            /* Leemos y verificamos que el issn exista */
            issn = getAttrValueInTag("article", "issn");
            /* Hacemos un segundo intento con la etiqueta text en caso de que no haya aparecido con article */
            if (issn == null) {
                issn = this.getAttrValueInTag("text", "issn");
                this.dtdType = "text";
            }
            if (log.IsDebugEnabled) log.Debug("issn: " + issn);
            if (issn == null) {
                MessageBox.Show(Resources.RegexMarkup_issnNotDefined, Resources.RegexMarkup_title);
            } else {
                /*Asignamos el numero de version de la DTD*/
                this.dtdVersion = this.getAttrValueInTag(this.dtdType, "version");
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
                    /* Asignamos el estilo de la citación */
                    this.citationStyle = objElem.SelectSingleNode("norm").InnerText.Trim();
                    /* Asignamos los struct en los que se divide la revista */
                    structNode = objElem.SelectSingleNode("struct");
                    /* Patrón de búsqueda */
                    
                    patternString = objElem.SelectSingleNode("regex").InnerText;
                    //MessageBox.Show("Pattern String");
                    /* Verificamos que la seleccion sea del parrafo completo */
                    Word.Range start = Globals.ThisAddIn.Application.Selection.Range;
                    docSeleccion = Globals.ThisAddIn.Application.Selection;
                    if (docSeleccion.Start != docSeleccion.Paragraphs.First.Range.Start || docSeleccion.End != docSeleccion.Paragraphs.Last.Range.End){
                        MessageBox.Show(Resources.RegexMarkup_selectCitationComplete, Resources.RegexMarkup_title);
                    } else {
                        /* Asignamos el texto de la seleccion a subjectString */
                        if (log.IsInfoEnabled) log.Info("Select paragraphs");
                        subjetcString = docSeleccion.Range.Text;
                        /* Inicializando arratlist de citas */
                        citas = new List<MarkupStruct>();
                        /*Creamos una instancia del formulario*/
                        this.formValidate = ValidateMarkup.Instance;
                        this.formValidate.CitationStyle = this.citationStyle;
                        this.formValidate.DtdVersion = this.dtdVersion;
                        this.formValidate.DtdType = this.dtdType;
                        this.formValidate.updateDTDInfo();
                        /* Buscando parrafo por parrafo */
                        foreach (Word.Paragraph parrafo in docSeleccion.Paragraphs){
                            /* Mandamos el texto de cada parrafo a una funcion que nos lo regresara marcado y quitamos el salto linea */
                            object parrafoStart = parrafo.Range.Start;
                            object parrafoEnd = (parrafo.Range.End - 1);
                            subjetcString = ActiveDocument.Range(ref parrafoStart, ref parrafoEnd).Text;
                            //MessageBox.Show(subjetcString, "Texto de parrafo");
                            if (subjetcString != null)
                            {   
                                marked = true;
                                /*Verificamos si la cadena original ya esta marcada*/
                                if (subjetcString.IndexOf("[") == 0 && subjetcString.LastIndexOf("]") == subjetcString.Length - 1)
                                {
                                    fixedMarkedString = subjetcString;
                                    clearTag = fixedMarkedString.Substring(1, fixedMarkedString.IndexOf("]") - 1);
                                    /*Nos aseguramos de que la etiqueta no contenga algun atributo buscando un espacio*/
                                    if(clearTag.IndexOf(" ") > 0){
                                        clearTag = clearTag.Substring(clearTag.IndexOf(" "));
                                    }
                                    /*Con la ayuda de la instancia del formulario limpiamos la cadena marcada*/
                                    this.tags.getChilds(clearTag);
                                    subjetcString = this.formValidate.clearTag(fixedMarkedString, clearTag);
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
                                    if (subjetcString == markedString)
                                    {
                                        marked = false;
                                    }
                                    else {
                                        /* Relizamos un proceso para verificar que la cita marca sea la misma que la original y agregamos cadenas faltantes*/
                                        fixedMarkedString = subjetcString;
                                        /* Se comparara el texto comprendido entre etiquetas*/
                                        matchResults = objRegExp.Match(markedString);
                                        matchResults2 = matchResults.NextMatch();
                                        /* Declaramos una varible para contemplar la longitud de carateres que no estan en la cita marcada */
                                        int startDiffAdd = 0;
                                        while (matchResults.Success)
                                        {
                                            try
                                            {
                                                /* Declaramos varables para saber el final de la etiqueta 1 y el principio de la etiqueta 2*/
                                                int startMatch = matchResults.Index + startDiffAdd;
                                                int end = startMatch + matchResults.Length;
                                                int startMatch2 = matchResults2.Index + startDiffAdd;
                                                /* Insertamos la etiqueta en su lugar correspondinete con su respectiva diferencia */
                                                fixedMarkedString = fixedMarkedString.Insert(startMatch, matchResults.Value);
                                                /* Verificamos si el contenido entre etiquetas es el mismo en la etiqueta marcada y en la reconstruida */
                                                if (end < startMatch2 && fixedMarkedString.Substring(end, (startMatch2 - end)) != markedString.Substring(end - startDiffAdd, (startMatch2 - end)))
                                                {
                                                    String strOriginal = fixedMarkedString.Substring(end, (startMatch2 - end));
                                                    String strMarked = markedString.Substring(end - startDiffAdd, (startMatch2 - end));
                                                    String searchDiff = fixedMarkedString.Substring(end);
                                                    /* Verificamos donde inicia la diferencia entre las etiquetas*/
                                                    int startDiff = searchDiff.IndexOf(strMarked) + end; 
                                                    if (startDiff > 0)
                                                    {
                                                        /* Colocamos la etiqueta en su lugar adecuado en la cita reconstruida */
                                                        fixedMarkedString = fixedMarkedString.Remove(startMatch, matchResults.Length);
                                                        fixedMarkedString = fixedMarkedString.Insert((startDiff - matchResults.Length), matchResults.Value);
                                                        /* Agregamos la longitud de los carateres que se encuentran ya en la cadena reconstruida */
                                                        startDiffAdd += fixedMarkedString.Substring(end, (startDiff - end)).Length;
                                                    }
                                                    
                                                }
                                                matchResults = matchResults.NextMatch();
                                                matchResults2 = matchResults2.NextMatch();
                                            }
                                            catch (Exception e) {
                                                MessageBox.Show(e.Message);
                                            }
                                        }
                                    }
                                }
                                citas.Add(new MarkupStruct(subjetcString, fixedMarkedString, marked, ActiveDocument.Range(ref parrafoStart, ref parrafoEnd)));
                            }
                        }
                        /* Mandamos llamar al formulario para la validación de las citas*/
                        this.formValidate.startValidate(ref citas);
                        if (this.formValidate.ShowDialog() == DialogResult.OK)
                        {
                            /* Ocultamos la aplicacion durante los procesos de reemplazo y coloreado para hacer mas rapida la aplicacion */
                            Globals.ThisAddIn.Application.Visible = false;
                            waitForm = Waiting.Instance;
                            waitForm.Show();
                            /* Reemplzando texto original por el marcado */
                            /* Utilizando el rango de texto de la cita original y reemplzado el texto por el marcado */
                            foreach (MarkupStruct cita in citas)
                            {
                                if (cita.Marked && cita.Colorized)
                                {
                                    Clipboard.Clear();
                                    cita.MarkedRtb.SelectAll();
                                    Clipboard.SetText(cita.MarkedRtb.SelectedRtf, TextDataFormat.Rtf);
                                    cita.RngCita.Paste();
                                    Clipboard.Clear();
                                }
                            }
                            /* Mostramos de nuevo la aplicacion */
                            waitForm.Hide();
                            Globals.ThisAddIn.Application.Visible = true;
                            /*Guardamos los cambios*/
                            if (!ActiveDocument.ReadOnly)
                            {
                                object FileName = ActiveDocument.Name;
                                object FileFormat = Word.WdSaveFormat.wdFormatFilteredHTML;
                                ActiveDocument.SaveAs(ref FileName, ref FileFormat, ref missing,
                                        ref missing, ref missing, ref missing,
                                        ref missing, ref missing,
                                        ref missing, ref missing,
                                        ref missing, ref missing, ref missing,
                                        ref missing, ref missing, ref missing);
                            }
                            else
                            {
                                MessageBox.Show(Resources.RegexMarkup_messageOnlyRead);
                            }
                        }
                        
                    }

                }

            }
            if (log.IsInfoEnabled) log.Info("End");
        }
        #endregion

        #region getAttrValueInTag
        /// <summary>
        /// Función para obtener el atributo de una etiqueta(TAG)
        /// </summary>
        /// <param name="tag">Etiqueta donde buscaremos</param>
        /// <param name="attr">Atributo a buscar</param>
        /// <returns>El valor del atributo buscado</returns>
        private String getAttrValueInTag(String tag, String attr) {
            if (log.IsInfoEnabled) log.Info("Begin");
            if (log.IsDebugEnabled) log.Debug("getAttrValueInTag(tag: " + tag + ", attr: " + attr + ")");
            /* Declaración de variables */
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
            if (log.IsInfoEnabled) log.Info("End");
            return result;
        }
        #endregion
   
        #region markupText
        /// <summary>
        /// Función para marcar la cadena de texto
        /// </summary>
        /// <param name="refPattern">Expresión regular</param>
        /// <param name="refString">Cadena para buscar coincidencias</param>
        /// <param name="refStruct">Instrucciones de marcado</param>
        /// <returns>La cadena marcada</returns>
        private String markupText(String refPattern, String refString, XmlNode refStruct) {
            if (log.IsInfoEnabled) log.Info("Begin");
            if (log.IsDebugEnabled) log.Debug("markupText(refPattern: " + refPattern + ", refString: " + refString + ", refStruct: " + refStruct.ToString() + ")");
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
            RegexOptions options = RegexOptions.None;
            Match matchResults = null;
            Match multipleOptionsMarchResults = null;
            XmlNode structNode = null;
            XmlNodeList multipleOptions = null;
            XmlNode singleOptionStruct = null;
            /* Verificamos si la cadena es nula antes de evaluarla */
            if (refString != null)
            {
                /* Iniciando búsqueda del patron en la cadena de texto */
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
                            /* Verificamos que el nodo hijo sea diferente de prevalue y postvalue */
                            if (itemXML.Name != "prevalue" && itemXML.Name != "postvalue")
                            {
                                /* Verificamos si el nodo es una etiqueta(tag) o no */
                                if (itemXML.Attributes.GetNamedItem("tag") != null)
                                {
                                    /*Verificamos y agregamos atributos por omisión  a la etiqueta de apertura*/
                                    if (itemXML.SelectSingleNode("attr") == null)
                                    {
                                        tagStringOpen = "[" + itemXML.Name + "]";
                                    }
                                    else
                                    {
                                        tagStringOpen = "[" + itemXML.Name;
                                        foreach (XmlNode attrNode in itemXML.SelectSingleNode("attr"))
                                        {
                                            if (attrNode.Name == "dateiso" && attrNode.InnerText == "")
                                            {
                                                tagStringOpen += " " + attrNode.Name + "=\"" + objRegExp.Replace(refString, itemXML.SelectSingleNode("value").InnerText) + "0000\"";
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
                                    /* Verificamos si hay que poner un valor antes de la etiqueta(tag) */
                                    if (itemXML.SelectSingleNode("prevalue") != null)
                                    {
                                        backreferencePreValue = itemXML.SelectSingleNode("prevalue").InnerText;
                                        backreferencePreValueString = objRegExp.Replace(matchResults.Value, backreferencePreValue);

                                        resultString += backreferencePreValueString;

                                    }
                                    /* Si esta compuesto de otras etiquetas(tag) volvemos a enviar la cadena y el patron con los nodos hijos de la etiqueta(tag) */
                                    resultString += tagStringOpen + this.markupText(refPattern, refString, itemXML) + tagStringClose;
                                    /* Verificamos si hay que poner un valor despues de la etiqueta(tag) */
                                    if (itemXML.SelectSingleNode("postvalue") != null)
                                    {
                                        backreferencePostValue = itemXML.SelectSingleNode("postvalue").InnerText;
                                        backreferencePostValueString = objRegExp.Replace(matchResults.Value, backreferencePostValue);
                                        resultString += backreferencePostValueString;
                                    }
                                }
                                else
                                {
                                    /* Deacuerdo al valor que tenemos indicado en el xml extramos la cadena de texto correspondiente */
                                    backreference = itemXML.SelectSingleNode("value").InnerText;
                                    subjectString = objRegExp.Replace(matchResults.Value, backreference);
                                    /* Verificamos si a la cadena de texto resultante tiene multiples opciones, hay  que aplicarle un patron nuevo ó si agregamos las etiquetas(tag) directamente */
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
            if (log.IsDebugEnabled) log.Debug("resultString: " + resultString);
            if (log.IsInfoEnabled) log.Info("End");
            return resultString;
        }
        #endregion

    }
}
