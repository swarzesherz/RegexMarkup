using RegexMarkup.Properties;
namespace RegexMarkup
{
    partial class RibbonRegexMarkup
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RibbonRegexMarkup));
            this.tab1 = new Microsoft.Office.Tools.Ribbon.RibbonTab();
            this.groupRegexMarkup = new Microsoft.Office.Tools.Ribbon.RibbonGroup();
            this.buttonRegexMarkup = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.buttonConfig = new Microsoft.Office.Tools.Ribbon.RibbonButton();
            this.tab1.SuspendLayout();
            this.groupRegexMarkup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.groupRegexMarkup);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // groupRegexMarkup
            // 
            this.groupRegexMarkup.Items.Add(this.buttonRegexMarkup);
            this.groupRegexMarkup.Items.Add(this.buttonConfig);
            this.groupRegexMarkup.Label = "RegexMarkup";
            this.groupRegexMarkup.Name = "groupRegexMarkup";
            // 
            // buttonRegexMarkup
            // 
            this.buttonRegexMarkup.Image = ((System.Drawing.Image)(resources.GetObject("buttonRegexMarkup.Image")));
            this.buttonRegexMarkup.Label = "Markup Regex";
            this.buttonRegexMarkup.Name = "buttonRegexMarkup";
            this.buttonRegexMarkup.ShowImage = true;
            this.buttonRegexMarkup.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.buttonRegexMarkup_Click);
            // 
            // buttonConfig
            // 
            this.buttonConfig.Image = ((System.Drawing.Image)(resources.GetObject("buttonConfig.Image")));
            this.buttonConfig.Label = "Configuración";
            this.buttonConfig.Name = "buttonConfig";
            this.buttonConfig.ShowImage = true;
            this.buttonConfig.Click += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonControlEventArgs>(this.buttonConfig_Click);
            // 
            // RibbonRegexMarkup
            // 
            this.Name = "RibbonRegexMarkup";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tab1);
            this.Load += new System.EventHandler<Microsoft.Office.Tools.Ribbon.RibbonUIEventArgs>(this.RibbonRegexMarkup_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.groupRegexMarkup.ResumeLayout(false);
            this.groupRegexMarkup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupRegexMarkup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonRegexMarkup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonConfig;
    }

    partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonReadOnlyCollection
    {
        internal RibbonRegexMarkup RibbonRegexMarkup
        {
            get { return this.GetRibbon<RibbonRegexMarkup>(); }
        }
    }
}
