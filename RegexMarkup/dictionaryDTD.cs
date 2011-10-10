using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;

namespace RegexMarkup
{
    public sealed class dictionaryDTD
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static dictionaryDTD instance = null;
        static readonly object padlock = new object();
        dictionaryDTD()
        {
        }

        public static dictionaryDTD Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new dictionaryDTD();
                    }
                    return instance;
                }
            }
        }
        #endregion

        private Dictionary<String, Dictionary<String, String>> DTD31 = null;
        private Dictionary<String, Dictionary<String, String>> DTD40 = null;

        public Dictionary<String, Dictionary<String, String>> getDTD31()
        {
            this.populateDTD();
            return this.DTD31;
        }

        public Dictionary<String, Dictionary<String, String>> getDTD40()
        {
            this.populateDTD();
            return this.DTD40;
        }

        private void populateDTD() {
            Dictionary<String, String> DTD40APA = null;
            Dictionary<String, String> DTD40Common = null;

            /* Si la DTD31 variable es nula la inicalizamos */
            if (this.DTD31 == null) {

            }

            /* Si la DTD40 variable es nula la inicalizamos */
            if (this.DTD40 == null)
            {
                try
                {
                    /* Inicalizamos DTD40 */
                    this.DTD40 = new Dictionary<String, Dictionary<String, String>>();
                    /* Etiquetas comunes */
                    DTD40Common = new Dictionary<string, string>();
                    DTD40Common.Add("fname", "fname");
                    DTD40Common.Add("surname", "surname");
                    /* Tags APA */
                    DTD40APA = new Dictionary<string, string>();
                    DTD40APA.Add("text-ref", "text-ref"); 
                    DTD40APA.Add("citat", "pcitat");
                    DTD40APA.Add("contrib", "pcontrib");
                    DTD40APA.Add("author", "author");
                    DTD40APA.Add("corpauth", "corpauth");
                    DTD40APA.Add("title", "title");
                    DTD40APA.Add("subtitle", "subtitle");
                    DTD40APA.Add("contrib", "date");
                    DTD40APA.Add("serial", "piserial");
                    DTD40APA.Add("monog", "pmonog");
                    /* Tags */
                    this.DTD40.Add("apa", mergeDictionarty(DTD40APA, DTD40Common));
                    MessageBox.Show(this.DTD40["apa"]["author"]);
                }
                catch (Exception e) {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public Dictionary<String, String> mergeDictionarty(Dictionary<String, String> first, Dictionary<String, String> second){
            Dictionary<String, String> returnDictionary = null;
            returnDictionary = new Dictionary<String, String>();
            /* Agregando elemntos del primer diccionario */
            foreach (KeyValuePair<string, string> pair in first)
	        {
	            returnDictionary.Add(pair.Key, pair.Value);
	        }
            /* Agregando elementos del segundo diccionario */
            foreach (KeyValuePair<string, string> pair in second)
	        {
                if (second[pair.Key] == null) {
                    returnDictionary.Add(pair.Key, pair.Value);
                }
	        }
            return returnDictionary;
        }

    }

}
