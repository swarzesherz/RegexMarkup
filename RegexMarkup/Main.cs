using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;

namespace RegexMarkup
{
    public partial class ThisAddIn
    {
        Office.CommandBar commandBarMarkup;
        Office.CommandBarButton regexButton;
        RegexMarkup objectRegexMarkup = RegexMarkup.Instance;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {

            AddToolbar();
       
        }
        private void AddToolbar()
        {   /* Creando la barra Markup Scielo Mexico */
            if (commandBarMarkup == null)
            {
                commandBarMarkup = Application.CommandBars.Add("Markup SciELO México", 1, missing, true);
            }
            else {
                commandBarMarkup = Application.CommandBars["Markup SciELO México"];
            }
            /* Agregando botones a la barra Markup Scielo Mexico */
            try
            {
                regexButton = (Office.CommandBarButton)commandBarMarkup.Controls.Add(1, missing, missing, missing, missing);
                regexButton.Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                regexButton.Caption = "Markup Regex";
                regexButton.Tag = "Markup Regex";
                regexButton.FaceId = 2476;
                
                regexButton.Click += new Office._CommandBarButtonEvents_ClickEventHandler(objectRegexMarkup.startMarkup);
                commandBarMarkup.Visible = true;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region Código generado de VSTO

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion
    }
}
