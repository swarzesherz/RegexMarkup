using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using RegexMarkup.Properties;
using System.Globalization;
using System.Deployment.Application;
using System.IO;
using Microsoft.Win32;
using System.Reflection;

namespace RegexMarkup
{
    public partial class RibbonRegexMarkup : OfficeRibbon
    {
        private RegexMarkup objectRegexMarkup = RegexMarkup.Instance;
        private ConfigRegexMarkup configForm = null;

        public RibbonRegexMarkup()
        {
            /*Agregando icono para Add/Remove Software*/
            this.SetAddRemoveProgramsIcon();
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
            /*Verificando el número de versión*/
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment currDeploy = ApplicationDeployment.CurrentDeployment;
                Version pubVer = currDeploy.CurrentVersion;
                String displayVer = pubVer.Major.ToString() + "." + pubVer.Minor.ToString() + "." + pubVer.Build.ToString() + "." + pubVer.Revision.ToString();
                this.groupRegexMarkup.Label = AssemblyInfoHelper.Product + " v" + displayVer;
            }
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

        #region SetAddRemoveProgramsIcon
        /// <summary>
        /// Función que nos permite poner un icono en agregar o quitar programas
        /// </summary>
        private void SetAddRemoveProgramsIcon()
        {
            //only run if deployed 
            if (ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.IsFirstRun)
            {
                try
                {
                    string iconSourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "markup.ico");
                    if (!File.Exists(iconSourcePath))
                        return;

                    RegistryKey myUninstallKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
                    string[] mySubKeyNames = myUninstallKey.GetSubKeyNames();
                    int i = 0;
                    bool keyFound = false;
                    while (i < mySubKeyNames.Length && !keyFound) {
                        RegistryKey myKey = myUninstallKey.OpenSubKey(mySubKeyNames[i], true);
                        object myValue = myKey.GetValue("DisplayName");
                        if (myValue != null && myValue.ToString() == AssemblyInfoHelper.Product)
                        {
                            myKey.SetValue("DisplayIcon", iconSourcePath);
                            myKey.SetValue("Publisher", AssemblyInfoHelper.Company);
                            keyFound = true;
                        }
                        i++;
                    }
                }
                catch (Exception e) { 
                }
            }
        }
        #endregion

    }
}
