namespace RegexMarkup
{
    partial class RibbonRegexMarkup : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public RibbonRegexMarkup()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.groupRegexMarkup = this.Factory.CreateRibbonGroup();
            this.buttonRegexMarkup = this.Factory.CreateRibbonButton();
            this.buttonDebug = this.Factory.CreateRibbonButton();
            this.buttonConfig = this.Factory.CreateRibbonButton();
            this.buttonInstitution = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.groupRegexMarkup.SuspendLayout();
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
            this.groupRegexMarkup.Items.Add(this.buttonDebug);
            this.groupRegexMarkup.Items.Add(this.buttonConfig);
            this.groupRegexMarkup.Items.Add(this.buttonInstitution);
            this.groupRegexMarkup.Label = "RegexMarkup";
            this.groupRegexMarkup.Name = "groupRegexMarkup";
            // 
            // buttonRegexMarkup
            // 
            this.buttonRegexMarkup.Image = global::RegexMarkup.Properties.Resources.markup;
            this.buttonRegexMarkup.Label = "Markup Regex";
            this.buttonRegexMarkup.Name = "buttonRegexMarkup";
            this.buttonRegexMarkup.ShowImage = true;
            this.buttonRegexMarkup.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonRegexMarkup_Click);
            // 
            // buttonDebug
            // 
            this.buttonDebug.Image = global::RegexMarkup.Properties.Resources.debug;
            this.buttonDebug.Label = "Bítacora";
            this.buttonDebug.Name = "buttonDebug";
            this.buttonDebug.ShowImage = true;
            this.buttonDebug.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonDebug_Click);
            // 
            // buttonConfig
            // 
            this.buttonConfig.Image = global::RegexMarkup.Properties.Resources.config;
            this.buttonConfig.Label = "Configuración";
            this.buttonConfig.Name = "buttonConfig";
            this.buttonConfig.ShowImage = true;
            this.buttonConfig.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonConfig_Click);
            // 
            // buttonInstitution
            // 
            this.buttonInstitution.Image = global::RegexMarkup.Properties.Resources.institution;
            this.buttonInstitution.Label = "Institución";
            this.buttonInstitution.Name = "buttonInstitution";
            this.buttonInstitution.ShowImage = true;
            this.buttonInstitution.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonInstitution_Click);
            // 
            // RibbonRegexMarkup
            // 
            this.Name = "RibbonRegexMarkup";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.RibbonRegexMarkup_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.groupRegexMarkup.ResumeLayout(false);
            this.groupRegexMarkup.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupRegexMarkup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonRegexMarkup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonDebug;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonConfig;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonInstitution;
    }

    partial class ThisRibbonCollection
    {
        internal RibbonRegexMarkup Ribbon1
        {
            get { return this.GetRibbon<RibbonRegexMarkup>(); }
        }
    }
}
