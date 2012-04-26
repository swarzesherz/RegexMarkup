using System;
using System.Collections.Generic;
using Sgml;

namespace RegexMarkup
{
    public class AttrTagStruct
    {
        private String name = null;
        /*Dictionary<Idioma, Dictionary<clave, valor>>*/
        private Dictionary<String, Dictionary<String, String>> values = null;
        private AttributePresence presence;
        private AttributeType type;

        public AttributeType Type
        {
            get { return type; }
            set { type = value; }
        }

        public AttributePresence Presence
        {
            get { return presence; }
            set { presence = value; }
        }

        public Dictionary<String, Dictionary<String, String>> Values
        {
            get { 
                return this.values; 
            }
            set { 
                this.values = value; 
            }
        }


        public String Name
        {
            get { 
                return this.name; 
            }
            set { 
                this.name = value; 
            }
        }
    }
}
