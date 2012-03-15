﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Sgml;

namespace RegexMarkup
{
    public sealed class DTDSciELO
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static DTDSciELO instance=null;
        static readonly object padlock = new object();


        DTDSciELO()
        {
        }

        public static DTDSciELO Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new DTDSciELO();
                    }
                    return instance;
                }
            }
        }
        #endregion

        private Dictionary<String, Dictionary<String, SgmlDtd>> version = new Dictionary<string, Dictionary<string, SgmlDtd>>();
        private Dictionary<String, String> availableVersion = new Dictionary<string, string>();

        public SgmlDtd getDTD(String version, String DTD) {
            SgmlReader reader = null;
            Dictionary<String, SgmlDtd> dtd = null;
            if (this.checkAvailableVersion(DTD+version) && !this.version.ContainsKey(version)) { 
                reader = new SgmlReader();
                reader.CaseFolding = Sgml.CaseFolding.ToLower;
                String sgmlArticle = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.availableVersion[DTD+version]);
                reader.SystemLiteral = sgmlArticle;
                dtd = new Dictionary<String, SgmlDtd>();
                dtd.Add(DTD, reader.Dtd);
                this.version.Add(version, dtd);
            }

            return this.version[version][DTD];
        }

        private bool checkAvailableVersion(String version) {
            if (this.availableVersion.Count <= 0)
            {
                this.availableVersion.Add("article4.0", "SGML/art4_0.dtd");
                this.availableVersion.Add("text4.0", "SGML/text4_0.dtd");
            }
            return this.availableVersion.ContainsKey(version);   
        }
    }
}