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
            this.tabRegexMarkup = this.Factory.CreateRibbonTab();
            this.groupRegexMarkup = this.Factory.CreateRibbonGroup();
            this.buttonRegexMarkup = this.Factory.CreateRibbonButton();
            this.buttonDebug = this.Factory.CreateRibbonButton();
            this.buttonConfig = this.Factory.CreateRibbonButton();
            this.buttonInstitution = this.Factory.CreateRibbonButton();
            this.mmlmath = this.Factory.CreateRibbonButton();
            this.mmlmathfull = this.Factory.CreateRibbonButton();
            this.revert_equation = this.Factory.CreateRibbonButton();
            this.remove_hyperlinks = this.Factory.CreateRibbonButton();
            this.tabRegexMarkup.SuspendLayout();
            this.groupRegexMarkup.SuspendLayout();
            // 
            // tabRegexMarkup
            // 
            this.tabRegexMarkup.Groups.Add(this.groupRegexMarkup);
            this.tabRegexMarkup.Label = "RegexMarkup";
            this.tabRegexMarkup.Name = "tabRegexMarkup";
            // 
            // groupRegexMarkup
            // 
            this.groupRegexMarkup.Items.Add(this.buttonRegexMarkup);
            this.groupRegexMarkup.Items.Add(this.buttonDebug);
            this.groupRegexMarkup.Items.Add(this.buttonConfig);
            this.groupRegexMarkup.Items.Add(this.buttonInstitution);
            this.groupRegexMarkup.Items.Add(this.mmlmath);
            this.groupRegexMarkup.Items.Add(this.mmlmathfull);
            this.groupRegexMarkup.Items.Add(this.revert_equation);
            this.groupRegexMarkup.Items.Add(this.remove_hyperlinks);
            this.groupRegexMarkup.Label = "RegexMarkup";
            this.groupRegexMarkup.Name = "groupRegexMarkup";
            // 
            // buttonRegexMarkup
            // 
            this.buttonRegexMarkup.Image = global::RegexMarkup.Properties.Resources.regex;
            this.buttonRegexMarkup.Label = "RegexMarkup";
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
            // mmlmath
            // 
            this.mmlmath.Label = "equation";
            this.mmlmath.Name = "mmlmath";
            this.mmlmath.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.mmlmath_Click);
            // 
            // mmlmathfull
            // 
            this.mmlmathfull.Label = "*equation";
            this.mmlmathfull.Name = "mmlmathfull";
            this.mmlmathfull.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.mmlmathfull_Click);
            // 
            // revert_equation
            // 
            this.revert_equation.Label = "*revert equation";
            this.revert_equation.Name = "revert_equation";
            this.revert_equation.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.revert_equation_Click);
            // 
            // remove_hyperlinks
            // 
            this.remove_hyperlinks.Label = "remove hyperlinks";
            this.remove_hyperlinks.Name = "remove_hyperlinks";
            this.remove_hyperlinks.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.remove_hyperlinks_Click);
            // 
            // RibbonRegexMarkup
            // 
            this.Name = "RibbonRegexMarkup";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tabRegexMarkup);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.RibbonRegexMarkup_Load);
            this.tabRegexMarkup.ResumeLayout(false);
            this.tabRegexMarkup.PerformLayout();
            this.groupRegexMarkup.ResumeLayout(false);
            this.groupRegexMarkup.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tabRegexMarkup;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupRegexMarkup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonRegexMarkup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonDebug;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonConfig;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonInstitution;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton mmlmath;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton mmlmathfull;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton revert_equation;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton remove_hyperlinks;
    }

    partial class ThisRibbonCollection
    {
        internal RibbonRegexMarkup Ribbon1
        {
            get { return this.GetRibbon<RibbonRegexMarkup>(); }
        }
    }
}
