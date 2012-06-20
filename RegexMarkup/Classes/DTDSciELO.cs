using System;
using System.Collections.Generic;
using System.IO;
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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SgmlDtd getDTD(String version, String DTD) {
            if (log.IsDebugEnabled) log.Debug("getDTD(version: " + version + ", DTD: " + DTD + ")");
            SgmlReader reader = null;
            Dictionary<String, SgmlDtd> dtd = null;
            if (this.checkAvailableVersion(DTD+version) && !this.version.ContainsKey(version)) { 
                reader = new SgmlReader();
                reader.CaseFolding = Sgml.CaseFolding.ToLower;
                String sgmlArticle = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.availableVersion[DTD+version]);
                if (log.IsDebugEnabled) log.Debug("sgmlArticle: " + sgmlArticle);
                reader.SystemLiteral = sgmlArticle;
                dtd = new Dictionary<String, SgmlDtd>();
                dtd.Add(DTD, reader.Dtd);
                if (log.IsDebugEnabled) log.Debug("dtd.Add(DTD: " + DTD + ", reader.Dtd: " + reader.Dtd.ToString() + ")");
                this.version.Add(version, dtd);
                if (log.IsDebugEnabled) log.Debug("this.version.Add(version: " + version + ", dtd: " + dtd.ToString() + ")");
            }
            if (log.IsDebugEnabled) log.Debug("return this.version[version: " + version + "][DTD: " + DTD + "]");
            return this.version[version][DTD];
        }

        private bool checkAvailableVersion(String version) {
            if (log.IsDebugEnabled) log.Debug("checkAvailableVersion(version: " + version + ")");
            if (this.availableVersion.Count <= 0)
            {
                this.availableVersion.Add("article4.0", @"SGML\art4_0.dtd");
                this.availableVersion.Add("text4.0", @"SGML\text4_0.dtd");
            }
            return this.availableVersion.ContainsKey(version);   
        }
    }
}
