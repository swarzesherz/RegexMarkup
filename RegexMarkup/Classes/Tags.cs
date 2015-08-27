using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RegexMarkup.Properties;
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

        #region getDescription
        /// <summary>
        /// Obtiene la descripción de una etiqueta
        /// </summary>
        /// <param name="node">Etique de la que queremos el atributo</param>
        /// <returns></returns>
        public String getDescription(String node) {
            String newLineTag = "";
            String pathDescriptionFile = null;
            String language = Resources.Culture.TwoLetterISOLanguageName;
            StreamReader langReader = null;
            /*Si en la lista el idioma actual no existe lo agregamos*/
            if (!this.languageTags.Contains(language))
            {
                this.languageTags.Add(language);
                /*Lenamos el diccionario de etiquetas con el archivo xx_bars.tr(renombar a xx-XX_bars.tr) que se encuentra en C:\SciELO\bin\markup*/
                pathDescriptionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SGML/" + language + "_bars.tr");
                try
                {
                    /*Verificamos que el archivo de descripción exista de otra forma cargamos es_bars.tr como idioma principal*/
                    if (File.Exists(pathDescriptionFile))
                    {
                        langReader = new StreamReader(pathDescriptionFile, Encoding.Default);
                    }
                    else
                    {
                        pathDescriptionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SGML/es_bars.tr");
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

        #endregion

        public Dictionary<String, AttrTagStruct> getAttributes(String node)
        {
            Sgml.ElementDecl nodeElement = this.dtd.FindElement(node);
            String attrName = null;
            /*Si el nodo no existe en el diccionario lo agregamos*/
            if (!this.tag.ContainsKey(node)) {
                this.tag.Add(node, new TagStruct());
            }
            if (nodeElement.AttList != null && nodeElement.AttList.Count > 0 && this.tag[node].Attributes == null)
            {
                this.tag[node].Attributes = new Dictionary<String,AttrTagStruct>();
                /*Obteniendo la lista de atributos*/
                foreach(KeyValuePair<String, AttDef> attDef in nodeElement.AttList){
                    attrName = attDef.Value.Name.ToLower();
                    this.tag[node].Attributes.Add(attrName, new AttrTagStruct());
                    this.tag[node].Attributes[attrName].Name = attrName;
                    this.tag[node].Attributes[attrName].Presence = attDef.Value.AttributePresence;
                    this.tag[node].Attributes[attrName].Type = attDef.Value.Type;
                }
                this.setAttributeValues(node);
            }
            return this.tag[node].Attributes;
        }

        private void setAttributeValues(String node) {
            String language = Resources.Culture.TwoLetterISOLanguageName;
            StreamReader langReader = null;
            String pathAttributeFile = null;
            String lineAttr = "";
            String lineAttrKeys = null;
            String lineAttrValues = null;
            List<String> attrKeys = null;
            List<String> attrValues = null;

            if (!this.tag[node].AttributeLanguages.Contains(language))
            {
                this.tag[node].AttributeLanguages.Add(language);
                try
                {
                    /*Verificamos que el archivo de descripción exista de otra forma cargamos es_ES_bars.tr como idioma principal*/
                    pathAttributeFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SGML/" + language + "_attb.mds");
                    if (File.Exists(pathAttributeFile))
                    {
                        langReader = new StreamReader(pathAttributeFile, Encoding.Default);
                    }
                    else
                    {
                        pathAttributeFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SGML/es_attb.mds");
                        langReader = new StreamReader(pathAttributeFile, Encoding.Default);
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show(e.Message);
                }
                while (lineAttr != null)
                {
                    lineAttr = langReader.ReadLine();
                    if (lineAttr != null && lineAttr.IndexOf(";") < 0)
                    {
                        if (this.tag[node].Attributes.ContainsKey(lineAttr))
                        {
                            lineAttrValues = langReader.ReadLine();
                            lineAttrKeys = langReader.ReadLine();
                            langReader.ReadLine();
                            if (lineAttrValues.IndexOf(";") > 0)
                            {
                                attrValues = new List<string>(lineAttrValues.Split(';'));
                                attrKeys = new List<string>(lineAttrKeys.Split(';'));
                            }
                            else
                            {
                                attrValues = new List<string>();
                                attrKeys = new List<string>();
                                attrValues.Add(lineAttrValues);
                                attrKeys.Add(lineAttrKeys);
                            }
                            /*Si es necesario instaciamos el diccionario para idioma*/
                            if (this.tag[node].Attributes[lineAttr].Values == null) {
                                this.tag[node].Attributes[lineAttr].Values = new Dictionary<string, Dictionary<string, string>>();
                            }
                            /*Si el idioma no existe en el diccionario lo agregamos*/
                            if (!this.tag[node].Attributes[lineAttr].Values.ContainsKey(language)) {
                                this.tag[node].Attributes[lineAttr].Values.Add(language, new Dictionary<string, string>());
                                /*Agregamos los valores del atributo*/
                                for (int i = 0; i < attrKeys.Count; i++)
                                {
                                    this.tag[node].Attributes[lineAttr].Values[language].Add(attrKeys[i], attrValues[i]);
                                }
                            }
                        }
                        else
                        {
                            langReader.ReadLine();
                            langReader.ReadLine();
                            langReader.ReadLine();
                        }
                    }
                }
                langReader.Close();
            }
        }

        #region getChilds
        /// <summary>
        /// Obtiene una lista de los nodos hijos de una etiqueta
        /// </summary>
        /// <param name="node">Nodo padre</param>
        /// <returns></returns>
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
        
        #endregion

        #region setChilds
        /// <summary>
        /// Busca y asigna los nodos hijos al valor por referencia childs
        /// </summary>
        /// <param name="model">Grupo en el cual encontramos los nodos hijos</param>
        /// <param name="childs">Volar por referencia donde se alamacenaran los nodos hijos</param>
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
        #endregion

        #region Getters and Setters
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
        #endregion

    }
}
