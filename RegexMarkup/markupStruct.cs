using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace RegexMarkup
{
    public class markupStruct
    {
        private String originalStr = null;
        private RichTextBox markedRtb = null;
        private String breakLines = null;
        private Boolean marked = false;
        private Boolean colorized = false;

        public markupStruct(String originalStr, String markedStr, Boolean marked) {
            this.originalStr = originalStr;
            /* Creando string rtf */
            this.markedRtb = new RichTextBox();
            this.markedRtb.Text = markedStr;
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
                return this.markedRtb.Text;
            }
            set {
                this.markedRtb.Text = value;
            }
        }

        public String MarkedStrRtf
        {
            get
            {
                return this.markedRtb.Rtf;
            }
            set
            {
                this.markedRtb.Rtf = value;
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

        public Boolean Colorized
        {
            get
            {
                return this.colorized;
            }
            set
            {
                this.colorized = value;
            }
        }
    }
}
