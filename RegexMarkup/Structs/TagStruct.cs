using System;
using System.Collections.Generic;

namespace RegexMarkup
{
    class TagStruct
    {
        private String name = null;
        /*Dictionary<Idioma, Descripcion>*/
        private Dictionary<String, String> description = null;
        private List<String> childs = null;
        private bool childNodes = false;
        private Dictionary<String, AttrTagStruct> attributes = null;
        private List<String> atributeLanguages = new List<string>();

        public List<String> AtributeLanguages
        {
            get { return atributeLanguages; }
            set { atributeLanguages = value; }
        }

        public Dictionary<String, AttrTagStruct> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public Dictionary<String, String> Description
        {
            get { return description; }
            set { description = value; }
        }

        public List<String> Childs
        {
            get { return childs; }
            set { childs = value; }
        }

        public bool ChildNodes
        {
            get { return childNodes; }
            set { childNodes = value; }
        }
    }
}
