using System;
using System.Deployment.Application;
using System.Globalization;
using System.IO;
using log4net;
using log4net.Appender;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Win32;
using RegexMarkup.Forms;
using RegexMarkup.Properties;

namespace RegexMarkup
{
    public partial class RibbonRegexMarkup
    {

        private RegexMarkup objectRegexMarkup = RegexMarkup.Instance;
        private ConfigRegexMarkup configForm = null;
        private Debug debugForm = null;
        private FindInstitution findInstitution = null;
        private Ecuation2Markup ecuation2Markup = null;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private void RibbonRegexMarkup_Load(object sender, RibbonUIEventArgs e)
        {
            /*Actualizando configuración de log4net*/
            this.updateSourceLogFiles();
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

        private void buttonRegexMarkup_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                this.objectRegexMarkup.startMarkup();
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void buttonDebug_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                this.debugForm = Debug.Instance;
                debugForm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
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

        private void buttonInstitution_Click(object sender, RibbonControlEventArgs e)
        {
            this.findInstitution = FindInstitution.Instance;
            findInstitution.ShowDialog();
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
                    while (i < mySubKeyNames.Length && !keyFound)
                    {
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
                catch (Exception e)
                {
                    if (log.IsErrorEnabled) log.Error(e.Message + "\r\n" + e.StackTrace);
                }
            }
        }
        #endregion

        #region  updateSourceLogFiles
        /// <summary>
        /// Función para actualizar la configuración de lo4net
        /// </summary>
        private void updateSourceLogFiles()
        {
            /*Actualizando directorios para los archivos usados por log4net*/
            String dataDirectory = null;
            String appenderType = null;
            String pathLogFile = null;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                dataDirectory = ApplicationDeployment.CurrentDeployment.DataDirectory;
            }
            else
            {
                dataDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            foreach (Object appender in LogManager.GetRepository().GetAppenders())
            {
                appenderType = appender.GetType().Name;
                log.Debug("appenderType = " + appenderType);
                switch (appenderType)
                {
                    case "RollingFileAppender":
                        pathLogFile = Path.Combine(dataDirectory, @"logs\RegexMarkup.xml");
                        log.Debug("((RollingFileAppender)appender).File = " + ((RollingFileAppender)appender).File);
                        ((RollingFileAppender)appender).File = pathLogFile;
                        log.Debug("((RollingFileAppender)appender).File = " + ((RollingFileAppender)appender).File);
                        ((RollingFileAppender)appender).ActivateOptions();
                        log.Debug("((RollingFileAppender)appender).File = " + ((RollingFileAppender)appender).File);
                        break;
                }
            }
        }
        #endregion

        private void mmlmath_Click(object sender, RibbonControlEventArgs e)
        {
            this.ecuation2Markup = Ecuation2Markup.Instance;
            this.ecuation2Markup.initialize();
            this.ecuation2Markup.convertSelection();
        }

        private void mmlmathfull_Click(object sender, RibbonControlEventArgs e)
        {
            this.ecuation2Markup = Ecuation2Markup.Instance;
            this.ecuation2Markup.initialize();
            this.ecuation2Markup.convertAll();
        }
    }
}
