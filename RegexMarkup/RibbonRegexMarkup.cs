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
using log4net;
using log4net.Config;   
using RegexMarkup.Forms;
using log4net.Appender;

namespace RegexMarkup
{
    public partial class RibbonRegexMarkup : OfficeRibbon
    {
        private RegexMarkup objectRegexMarkup = RegexMarkup.Instance;
        private ConfigRegexMarkup configForm = null;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public RibbonRegexMarkup()
        {
            /*Actualizando directorios para los archivos usados por log4net*/
            String dataDirectory = null;
            String appenderType = null;
            dataDirectory = AppDomain.CurrentDomain.BaseDirectory;

            foreach (Object appender in LogManager.GetRepository().GetAppenders())
            {
                appenderType = appender.GetType().Name;
                log.Debug("appenderType = " + appenderType);
                switch (appenderType)
                {
                    case "RollingFileAppender":
                        log.Debug("((RollingFileAppender)appender).File = " + ((RollingFileAppender)appender).File);
                        ((RollingFileAppender)appender).File = ((RollingFileAppender)appender).File.Replace(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dataDirectory);
                        log.Debug("((RollingFileAppender)appender).File = " + ((RollingFileAppender)appender).File);
                        ((RollingFileAppender)appender).ActivateOptions();
                        log.Debug("((RollingFileAppender)appender).File = " + ((RollingFileAppender)appender).File);
                        break;
                    case "AdoNetAppender":
                        log.Debug("((AdoNetAppender)appender).ConnectionString = " + ((AdoNetAppender)appender).ConnectionString);
                        ((AdoNetAppender)appender).ConnectionString = ((AdoNetAppender)appender).ConnectionString.Replace(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dataDirectory);
                        log.Debug("((AdoNetAppender)appender).ConnectionString = " + ((AdoNetAppender)appender).ConnectionString);
                        ((AdoNetAppender)appender).ActivateOptions();
                        log.Debug("((AdoNetAppender)appender).ConnectionString = " + ((AdoNetAppender)appender).ConnectionString);
                        break;
                }
            }
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
            /*Actualizando textos*/
            this.buttonConfig.Label = Resources.RibbonRegexMarkup_buttonConfiguration;
            this.buttonDebug.Label = Resources.RibbonRegexMarkup_buttonDebug;
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
            try
            {
                this.objectRegexMarkup.startMarkup();
            }
            catch (Exception ex) {
                if (log.IsErrorEnabled) log.Error(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void buttonConfig_Click(object sender, RibbonControlEventArgs e)
        {
            this.configForm = ConfigRegexMarkup.Instance;
            this.configForm.ShowDialog();
            /*Actualizando textos*/
            this.buttonConfig.Label = Resources.RibbonRegexMarkup_buttonConfiguration;
            this.buttonDebug.Label = Resources.RibbonRegexMarkup_buttonDebug;
        }

        private void buttonDebug_Click(object sender, RibbonControlEventArgs e)
        {
            Debug debugForm = new Debug();
            debugForm.Show();
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
                            myKey.SetValue("UrlUpdateInfo", ApplicationDeployment.CurrentDeployment.UpdateLocation.AbsoluteUri);
                            keyFound = true;
                        }
                        i++;
                    }
                }
                catch (Exception e) {
                    if (log.IsErrorEnabled) log.Error(e.Message + "\r\n" + e.StackTrace);
                }
            }
        }
        #endregion

    }
}
