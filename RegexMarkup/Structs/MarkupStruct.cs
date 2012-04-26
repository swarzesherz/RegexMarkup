using System;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace RegexMarkup
{
    public class MarkupStruct
    {
        private String originalStr = null;
        private RichTextBox markedRtb = null;
        private Boolean parsed = false;
        private Boolean marked = false;
        private Boolean colorized = false;
        private Word.Range rngCita = null;

        public MarkupStruct(String originalStr, String markedStr, Word.Range rngCita, Boolean marked, Boolean parsed) {
            this.originalStr = originalStr;
            /* Creando string rtf */
            this.markedRtb = new RichTextBox();
            this.markedRtb.DetectUrls = false;
            this.markedRtb.Text = markedStr;
            this.marked = marked;
            this.parsed = parsed;
            this.rngCita = rngCita;
        }

        public String OriginalStr {
            get {
                return this.originalStr;
            }
            set {
                this.originalStr = value;
            }
        }

        public RichTextBox MarkedRtb
        {
            get { 
                return markedRtb; 
            }
            set { 
                markedRtb = value; 
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

        public Boolean Parsed
        {
            get { 
                return this.parsed; 
            }
            set { 
                this.parsed = value; 
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

        public Word.Range RngCita {
            get {
                return this.rngCita;
            }
            set {
                this.rngCita = value;
            }
        }
    }
}
