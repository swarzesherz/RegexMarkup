using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegexMarkup
{
    class MarkupButton
    {
        private Button markup = null;
        private Button childs = null;

        public Button Markup
        {
            get { 
                return this.markup;
            }
            set { 
                this.markup = value; 
            }
        }

        public Button Childs
        {
            get { 
                return this.childs; 
            }
            set { 
                this.childs = value; 
            }
        }


    }
}
