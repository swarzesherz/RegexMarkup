using System;
using System.Collections.Generic;
using System.Text;

namespace RegexMarkup
{
    class markupStruct
    {
        private String original = null;
        private String marked = null;

        public markupStruct(String original, String marked) {
            this.original = original;
            this.marked = marked;
        }

        public String Original {
            get {
                return this.original;
            }
            set {
                this.original = value;
            }
        }

        public String Marked {
            get {
                return this.marked;
            }
            set {
                this.marked = value;
            }
        }
    }
}
