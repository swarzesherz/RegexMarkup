using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using System.Xml;
using System.Text.RegularExpressions;

namespace RegexMarkup
{
    class RegexMarkup
    {
        public static Word.Document ActiveDocument = null;
        /* Procedimiento al que llamaremos para inciar el proceso de marcación */
        public void StartMarkup(Office.CommandBarButton ctrl, ref bool cancel)
        {
            /* Declaracion de variables */
            String patternString = null;
            String subjetcString = null;
            String replaceText = null;
            String issn = null;
            Word.Paragraphs parrafos = null;
            Word.Paragraph parrafo = null;
            XmlDocument xmlDoc = new XmlDocument();
            Exception errores = null;
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
                    xmlDoc.Load(@"C:\Documents and Settings\Herz\Mis documentos\Dropbox\SciELO_Files\Automatas\regex.xmla");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }
                MessageBox.Show("Startup RegexMarkup");
            }
        }

        /* Función para obtener el atributo de una etiqueta(TAG) */
        private String getAttrValueInTag(String tag, String attr) {
            /* Declaración de variables */
            String subjectString = null;
            String result = null;
            String pattern = null;
            Match matchResults = null;
            RegexOptions options = RegexOptions.IgnoreCase;
            /* Inicializamos variables */
            pattern = "\\[" + tag + ".*?" + attr + "=\"(.*?)\".*?\\]";
            subjectString = ActiveDocument.Content.Text;
            /* Verificamos que haya coincidencia */
            try
            {
                Regex regexObj = new Regex(pattern, options);
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
    }
}
