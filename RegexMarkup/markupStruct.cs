using System;
using System.Collections.Generic;
using System.Text;

namespace RegexMarkup
{
    public class markupStruct
    {
        private String originalStr = null;
        private String markedStr = null;
        private String breakLines = null;
        private Boolean marked = false;

        public markupStruct(String originalStr, String markedStr, Boolean marked) {
            this.originalStr = originalStr;
            this.markedStr = markedStr;
            this.marked = marked;
            this.breakLines = "\r";
        }

        public String OriginalStr {
            get {
                return this.originalStr;
            }
            set {
                this.originalStr = value;
            }
        }

        public String MarkedStr {
            get {
                return this.markedStr;
            }
            set {
                this.markedStr = value;
            }
        }

        public String BreakLines {
            get
            {
                return this.breakLines;
            }
            set
            {
                this.breakLines = value;
            }
        }

        public Boolean Marked
        {
            get
            {
                return this.marked;
            }
            set
            {
                this.marked = value;
            }
        }
        public Boolean MarkedNo
        {
            get
            {
                return !this.marked;
            }
        }
    }
}
