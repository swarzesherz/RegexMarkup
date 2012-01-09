using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Sgml;
using System.Windows.Forms;

namespace RegexMarkup
{
    class DTDStruct
    {
        private String name = null;
        private Dictionary<String, DTDStruct> childs = null;
        private DTDStruct parent = null;

        public DTDStruct() {
            /* Test Sgml */
            try
            {
                SgmlReader reader = new SgmlReader();
                reader.CaseFolding = Sgml.CaseFolding.ToLower;
                String sgmlArticle = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"SGML\art4_0.dtd");
                reader.SystemLiteral = sgmlArticle;
                SgmlDtd DTDScielo = reader.Dtd;
                ElementDecl article = DTDScielo.FindElement("OAUTHOR");
                ElementDecl ocitat = DTDScielo.FindElement("FNAME");
                getChilds();
                foreach(Object single in article.ContentModel.CurrentModel.CurrentMembers){
                    //MessageBox.Show(single.GetType().Namespace+"."+single.GetType().Name);
                    if (single.GetType().Namespace + "." + single.GetType().Name == "Sgml.Group")
                    {
                        //MessageBox.Show(subelements.ToString());
                        foreach (String elemnet in ((Sgml.Group)single).CurrentMembers)
                        {
                            MessageBox.Show(elemnet);

                        }
                    }
                    else {
                        MessageBox.Show((String) single);
                    }
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
        }
        #region
        /// <summary>
        /// Test function to get nodes and dephs from the root node
        /// </summary>
        /// <param name="Name"></param>
        /// 
        public void getChilds() { 
            
        }
        #endregion
        public DTDStruct(String Name) {
            this.name = Name;
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
            this.childs.Add(childName, childValue);
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
