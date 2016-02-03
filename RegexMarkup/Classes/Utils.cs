using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Word = Microsoft.Office.Interop.Word;

namespace RegexMarkup
{
    class RegexMarkupUtils
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static RegexMarkupUtils instance=null;
        static readonly object padlock = new object();


        RegexMarkupUtils()
        {
        }

        public static RegexMarkupUtils Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new RegexMarkupUtils();
                    }
                    return instance;
                }
            }
        }
        #endregion

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Word.Document ActiveDocument = null;
        public static Object missing = Type.Missing;

        #region getAttrValueInTag
        /// <summary>
        /// Función para obtener el atributo de una etiqueta(TAG)
        /// </summary>
        /// <param name="tag">Etiqueta donde buscaremos</param>
        /// <param name="attr">Atributo a buscar</param>
        /// <returns>El valor del atributo buscado</returns>

        public String getAttrValueInTag(String tag, String attr) {
            return this.getAttrValueInTag(tag, attr, null);
        }
        public String getAttrValueInTag(String tag, String attr, String subject)
        {
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
            ActiveDocument = Globals.ThisAddIn.Application.ActiveDocument;
            pattern = @"\[" + tag + ".*?" + attr + @"=("".*?""|'.*?'|[^\ \]]*).*?\]";
            pattern = "\\[" + tag + ".*?" + attr + "=(\\\".*?\\\"|\\'.*?\\'|[^\\ \\]]*).*?\\]";
            subjectString = ActiveDocument.Content.Text;
            if (subject != null)
                subjectString = subject;
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
            if (log.IsInfoEnabled) log.Info("End");
            return result;
        }
        #endregion
   
    }
}
