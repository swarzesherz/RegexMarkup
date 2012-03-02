using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Sgml;

namespace RegexMarkup
{
    class DTDStruct
    {
        private String name = null;
        private String description = null;
        private Dictionary<String, DTDStruct> childs = null;
        private DTDStruct parent = null;

        public static SgmlDtd DTDScielo { 
            get
            {
                SgmlReader reader = new SgmlReader();
                reader.CaseFolding = Sgml.CaseFolding.ToLower;
                String sgmlArticle = @"C:\Users\Herz\Dropbox\SciELO_Files\Automatas\RegexMarkup\RegexMarkup\SGML\art4_0.dtd";
                reader.SystemLiteral = sgmlArticle;
                return reader.Dtd;;
            }
        }

        public DTDStruct() {
            /* Test Sgml */
            try
            {
                this.name = "OCITAT";
                ElementDecl article = DTDStruct.DTDScielo.FindElement(this.name);
                if (article.ContentModel.CurrentModel.CurrentMembers.Count > 0)
                {
                    this.childs = new Dictionary<string, DTDStruct>();
                    this.getChilds(article.ContentModel.CurrentModel);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #region
        /// <summary>
        /// Test function to get nodes and dephs from the root node
        /// </summary>
        /// <param name="Name"></param>
        /// 
        private void getChilds(Sgml.Group model) {
            foreach (Object child in model.CurrentMembers) {
                if (child.GetType().Namespace + "." + child.GetType().Name == "Sgml.Group")
                {
                    //MessageBox.Show(subelements.ToString());
                    this.getChilds((Sgml.Group)child);
                }
                else
                {
                    //MessageBox.Show((String)child);
                    this.addChild(child.ToString(), new DTDStruct(child.ToString()));
                }
            }
        }
        #endregion
        public DTDStruct(String Name) {
            this.name = Name;
            /* Test Sgml */
            try
            {
                ElementDecl article = DTDStruct.DTDScielo.FindElement(Name);
                if (article.ContentModel.CurrentModel.CurrentMembers.Count > 0) {
                    this.childs = new Dictionary<string, DTDStruct>(); 
                    this.getChilds(article.ContentModel.CurrentModel);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public DTDStruct(String Name, DTDStruct Parent)
        {
            this.name = Name;
            this.parent = Parent;
        }
        public DTDStruct(String Name, Dictionary<String, DTDStruct> Childs)
        {
            this.name = Name;
            this.childs = Childs;
        }
        public DTDStruct(String Name,  DTDStruct Parent, Dictionary<String, DTDStruct> Childs)
        {
            this.name = Name;
            this.parent = Parent;
            this.childs = Childs;
        }

        public DTDStruct Parent {
            get {
                return this.parent;
            }
            set {
                this.parent = value;
            }
        }
        public Dictionary<String, DTDStruct> Childs {
            get {
                return this.childs;
            }
            set {
                this.childs = value;
            }
        }
        public void addChild(String childName, DTDStruct childValue)
        {
            if (!this.childs.ContainsKey(childName)) {
                this.childs.Add(childName, childValue);
            }
        }
        public String Name {
            get {
                return this.name;
            }
            set {
                this.name = value;
            }
        }
    }
}
