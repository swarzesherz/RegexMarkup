using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using Microsoft.Office.Tools.Word.Extensions;
using System.Deployment.Application;
using log4net;
using log4net.Appender;

namespace RegexMarkup
{
    public partial class ThisAddIn
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            String dataDirectory = null;
            String appenderType = null;
            dataDirectory = AppDomain.CurrentDomain.BaseDirectory;

            foreach (Object appender in LogManager.GetRepository().GetAppenders())
            {
                appenderType = appender.GetType().Name;
                log.Debug("appenderType = " + appenderType);
                switch (appenderType) { 
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
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {

        }

        #region Código generado por VSTO

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
