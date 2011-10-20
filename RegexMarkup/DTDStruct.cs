using System;
using System.Collections.Generic;
using System.Text;

namespace RegexMarkup
{
    class DTDStruct
    {
        private String name = null;
        private Dictionary<String, DTDStruct> childs = null;
        private DTDStruct parent = null;

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
