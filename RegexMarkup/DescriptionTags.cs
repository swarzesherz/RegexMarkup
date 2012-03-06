using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexMarkup.Properties;
using System.IO;

namespace RegexMarkup
{
    public sealed class DescriptionTags
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static DescriptionTags instance = null;
        static readonly object padlock = new object();


        DescriptionTags()
        {
        }

        public static DescriptionTags Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new DescriptionTags();
                    }
                    return instance;
                }
            }
        }
        #endregion

        /*Diccionario con la descripcion de la etiqueta
         *Dictionary<Language, Dictionary<Tag, Description>>
         */
        private Dictionary<String, Dictionary<String, String>> tagDescription = new Dictionary<string,Dictionary<string,string>>();

        
        
        public String getDescription(String tag) {
            String newLineTag = "";
            String pathDescriptionFile = null;
            String language = Resources.Culture.ToString();
            StreamReader langReader = null;
            /*Si el diccionario para el idioma no existe lo agregamos*/
            if (!this.tagDescription.ContainsKey(language)) {
                this.tagDescription.Add(language, new Dictionary<string,string>());
                /*Lenamos el diccionario de etiquetas con el archivo xx_bars.tr(renombar a xx-XX_bars.tr) que se encuentra en C:\SciELO\bin\markup*/
                pathDescriptionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SGML/" + language + "_bars.tr");
                try
                {
                    /*Verificamos que el archivo de descripción exista de otra forma cargamos es_ES_bars.tr como idioma principal*/
                    if (File.Exists(pathDescriptionFile))
                    {
                        langReader = new StreamReader(pathDescriptionFile, Encoding.Default);
                    }
                    else {
                        pathDescriptionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SGML/es-ES_bars.tr");
                        langReader = new StreamReader(pathDescriptionFile, Encoding.Default);
                    }
                }
                catch (Exception e) {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                while (newLineTag != null) {
                    newLineTag = langReader.ReadLine();
                    if (newLineTag != null && newLineTag.IndexOf(";") > 0) {
                        newLineTag = newLineTag.Substring(newLineTag.IndexOf(";") + 1).Trim();
                        /*Verificamos que la descripcion de la etiqueta no exista para agregarla*/
                        if (!this.tagDescription[language].ContainsKey(newLineTag)) {
                            this.tagDescription[language].Add(newLineTag, langReader.ReadLine());
                        }
                    }
                }
                langReader.Close();
            }
            /*Si la etiqueta existe en el diccionario devolvemos su descripción de lo contrario regregamos el nombre de la etiqueta*/
            if (this.tagDescription[language].ContainsKey(tag))
            {
                return this.tagDescription[language][tag];
            }
            else {
                return tag;
            }
        }

    }
}
