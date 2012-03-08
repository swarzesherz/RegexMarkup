using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegexMarkup
{
    class TagStruct
    {
        private String name = null;
        private Dictionary<String, String> description = null;
        private List<String> childs = null;
        private bool childNodes = false;

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
