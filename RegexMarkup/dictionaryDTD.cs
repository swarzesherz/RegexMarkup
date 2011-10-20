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

        private Dictionary<String, Dictionary<String, Dictionary<String, String>>> DTD31 = null;
        private Dictionary<String, Dictionary<String, Dictionary<String, String>>> DTD40 = null;

        public Dictionary<String, Dictionary<String, Dictionary<String, String>>> getDTD31()
        {
            this.populateDTD();
            return this.DTD31;
        }

        public Dictionary<String, Dictionary<String, Dictionary<String, String>>> getDTD40()
        {
            this.populateDTD();
            return this.DTD40;
        }

        private void populateDTD() {
            Dictionary<String, Dictionary<String, String>> DTD40APA = null;
            Dictionary<String, String> DTD40APAGeneral = null;
            Dictionary<String, String> DTD40APAContrib = null;
            Dictionary<String, String> DTD40APASerial = null;
            Dictionary<String, String> DTD40APAMonog = null;
            Dictionary<String, Dictionary<String, String>> DTD40VANCOUVER = null;
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
                    this.DTD40 = new Dictionary<String, Dictionary<String, Dictionary<String, String>>>();
                    /* Etiquetas comunes */
                    DTD40Common = new Dictionary<string, string>();
                    DTD40Common.Add("fname", "fname");
                    DTD40Common.Add("surname", "surname");
                    /* Tags APA */
                    DTD40APA = new Dictionary<string, Dictionary<String, String>>();
                    DTD40APAGeneral = new Dictionary<String, String>();

                    /* Agregando etiquetas generales */
                    DTD40APAGeneral.Add("citat", "pcitat");
                    DTD40APAGeneral.Add("contrib", "pcontrib");
                    DTD40APAGeneral.Add("text-ref", "text-ref");
                    DTD40APAGeneral.Add("serial", "piserial");
                    DTD40APAGeneral.Add("monog", "pmonog");
                    DTD40APA.Add("general", DTD40APAGeneral);
                    /* Etiquetas de contribucion */
                    DTD40APAContrib = new Dictionary<String, String>();
                    DTD40APAContrib.Add("author", "author");
                    DTD40APAContrib.Add("corpauth", "corpauth");
                    DTD40APAContrib.Add("date", "date");
                    DTD40APAContrib.Add("title", "title");
                    DTD40APAContrib.Add("subtitle", "subtitle");
                    DTD40APA.Add("contrib", DTD40APAContrib);
                    /* Etiquetas de Serial*/
                    DTD40APASerial = new Dictionary<String, String>();
                    DTD40APASerial.Add("title", "sertitle");
                    DTD40APASerial.Add("volid", "volid");
                    DTD40APASerial.Add("issueno", "issueno");
                    DTD40APASerial.Add("suppl", "suppl");
                    DTD40APASerial.Add("pages", "pages");
                    DTD40APASerial.Add("cited", "cited");
                    DTD40APASerial.Add("url", "url");
                    DTD40APASerial.Add("doi", "doi");
                    DTD40APA.Add("serial", DTD40APASerial);
                    /* Etiquetas de monografia */
                    DTD40APAMonog = new Dictionary<String, String>();
                    DTD40APAMonog.Add("author", "author");
                    DTD40APAMonog.Add("corpauth", "corpauth");
                    DTD40APAMonog.Add("date", "date");
                    DTD40APAMonog.Add("title", "title");
                    DTD40APAMonog.Add("subtitle", "subtitle");
                    DTD40APAMonog.Add("thesis", "thesis");
                    DTD40APAMonog.Add("extent", "extent");
                    DTD40APAMonog.Add("city", "city");
                    DTD40APAMonog.Add("coltitle", "coltitle");
                    DTD40APAMonog.Add("colvolid", "colvolid");
                    DTD40APAMonog.Add("pages", "pages");
                    DTD40APAMonog.Add("extent", "extent");
                    DTD40APAMonog.Add("edition", "edition");
                    DTD40APAMonog.Add("", "");
                    DTD40APAMonog.Add("", "");
                    DTD40APAMonog.Add("", "");
                    DTD40APAMonog.Add("", "");
                    
                    this.DTD40.Add("apa", DTD40APA);
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
                if (first[pair.Key] == null)
                {
                    returnDictionary.Add(pair.Key, pair.Value);
                }
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
