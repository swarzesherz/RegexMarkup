using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegexMarkup.Properties;
using System.IO;
using Sgml;

namespace RegexMarkup
{
    public sealed class Tags
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static Tags instance = null;
        static readonly object padlock = new object();


        Tags()
        {
        }

        public static Tags Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new Tags();
                    }
                    return instance;
                }
            }
        }
        #endregion

        private Dictionary<String, TagStruct> tag = new Dictionary<String, TagStruct>();
        private List<String> languageTags = new List<String>();
        private SgmlDtd dtd = null;

        public String getDescription(String node) {
            String newLineTag = "";
            String pathDescriptionFile = null;
            String language = Resources.Culture.ToString();
            StreamReader langReader = null;
            /*Si en la lista el idioma actual no existe lo agregamos*/
            if (!this.languageTags.Contains(language))
            {
                this.languageTags.Add(language);
                /*Lenamos el diccionario de etiquetas con el archivo xx_bars.tr(renombar a xx-XX_bars.tr) que se encuentra en C:\SciELO\bin\markup*/
                pathDescriptionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SGML/" + language + "_bars.tr");
                try
                {
                    /*Verificamos que el archivo de descripción exista de otra forma cargamos es_ES_bars.tr como idioma principal*/
                    if (File.Exists(pathDescriptionFile))
                    {
                        langReader = new StreamReader(pathDescriptionFile, Encoding.Default);
                    }
                    else
                    {
                        pathDescriptionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SGML/es-ES_bars.tr");
                        langReader = new StreamReader(pathDescriptionFile, Encoding.Default);
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                while (newLineTag != null)
                {
                    newLineTag = langReader.ReadLine();
                    if (newLineTag != null && newLineTag.IndexOf(";") > 0)
                    {
                        newLineTag = newLineTag.Substring(newLineTag.IndexOf(";") + 1).Trim();
                        /*Verificamos que exista la etiqueta en el diccionario si no, la agregamos*/
                        if(!this.tag.ContainsKey(newLineTag)){
                            this.tag.Add(newLineTag, new TagStruct());
                        }
                        this.tag[newLineTag].Description = new Dictionary<String, String>();
                        /*Verificamos que la descripcion de la etiqueta no exista para agregarla*/
                        if (!this.tag[newLineTag].Description.ContainsKey(language))
                        {
                            this.tag[newLineTag].Description.Add(language, langReader.ReadLine());
                        }
                    }
                }
                langReader.Close();
            }
            /*Si la etiqueta existe en el diccionario devolvemos su descripción de lo contrario agregamos el nombre de la etiqueta*/
            if (this.tag.ContainsKey(node) && this.tag[node].Description != null && this.tag[node].Description.ContainsKey(language))
            {
                return this.tag[node].Description[language];
            }
            else
            {
                return node;
            }
        }

        public List<String> getChilds(String node)
        {
            List<String> childs = null;
            /*Si el nodo no existe en el diccionario lo agregamos*/
            if (!this.tag.ContainsKey(node)) {
                this.tag.Add(node, new TagStruct());
            }
            /*Verificamos que el nodo tenga hijos*/
            Sgml.Group nodeGroup = this.dtd.FindElement(node).ContentModel.CurrentModel;
            if (nodeGroup.CurrentMembers.Count > 0 && this.tag[node].Childs == null)
            {
                childs = new List<string>();
                this.setChilds(nodeGroup, ref childs);
                this.tag[node].Childs = childs;
                this.tag[node].ChildNodes = true;
            }

            return this.tag[node].Childs;
        }

        private void setChilds(Sgml.Group model, ref List<String> childs)
        {
            String childName = null;
            foreach (Object child in model.CurrentMembers)
            {
                if (child.GetType().Namespace + "." + child.GetType().Name == "Sgml.Group")
                {
                    this.setChilds((Sgml.Group)child, ref childs);
                }
                else
                {
                    childName = child.ToString().ToLower();
                    /*Si el nodo no existe en la lista lo agregamos ademas tambien verificamos que exista tambien en el diccionario*/
                    if (!childs.Contains(childName))
                    {
                        childs.Add(childName);
                    }
                    if (!this.tag.ContainsKey(childName))
                    {
                        try
                        {
                            this.tag.Add(childName, new TagStruct());
                        }
                        catch (Exception e) {
                            System.Windows.Forms.MessageBox.Show(e.Message);
                        }
                    }
                    /*Verificamos si el nodo tiene hijos y si es asi lo marcamos*/
                    if (this.dtd.FindElement(childName).ContentModel.CurrentModel.CurrentMembers.Count > 0) { 
                        this.tag[childName].ChildNodes = true;
                    }
                }
            }
        
        }
        
        internal Dictionary<String, TagStruct> Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        public SgmlDtd Dtd
        {
            get { return dtd; }
            set { dtd = value; }
        }

    }
}
