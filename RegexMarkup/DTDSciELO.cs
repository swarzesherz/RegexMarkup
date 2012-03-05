using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Sgml;
using System.Deployment.Application;

namespace RegexMarkup
{
    class DTDSciELO
    {

        public static SgmlDtd Article { 
            get
            {
                SgmlReader reader = new SgmlReader();
                reader.CaseFolding = Sgml.CaseFolding.ToLower;
                String sgmlArticle = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SGML/art4_0.dtd");
                reader.SystemLiteral = sgmlArticle;
                return reader.Dtd;
            }
        }

        public static SgmlDtd Text
        {
            get
            {
                SgmlReader reader = new SgmlReader();
                reader.CaseFolding = Sgml.CaseFolding.ToLower;
                String sgmlText = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SGML/text4_0.dtd");
                reader.SystemLiteral = sgmlText;
                return reader.Dtd;
            }
        }
    }
}
