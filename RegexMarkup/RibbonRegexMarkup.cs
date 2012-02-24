using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using RegexMarkup.Properties;
using System.Globalization;

namespace RegexMarkup
{
    public partial class RibbonRegexMarkup : OfficeRibbon
    {
        private RegexMarkup objectRegexMarkup = RegexMarkup.Instance;
        private ConfigRegexMarkup configForm = null;

        public RibbonRegexMarkup()
        {
            /* Iniciando configuración de idioma */
            if (Settings.Default.language != "")
            {
                Resources.Culture = new CultureInfo(Settings.Default.language);
            }
            else
            {
                Resources.Culture = System.Globalization.CultureInfo.CurrentCulture;
                Settings.Default.language = Resources.Culture.ToString();
            }
            InitializeComponent();
            this.buttonConfig.Label = Resources.RibbonRegexMarkup_buttonConfiguration;
        }

        private void RibbonRegexMarkup_Load(object sender, RibbonUIEventArgs e)
        {
            
        }

        private void buttonRegexMarkup_Click(object sender, RibbonControlEventArgs e)
        {
            this.objectRegexMarkup.startMarkup();
        }

        private void buttonConfig_Click(object sender, RibbonControlEventArgs e)
        {
            this.configForm = ConfigRegexMarkup.Instance;
            this.configForm.ShowDialog(); 
        }
    }
}
