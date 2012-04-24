namespace RegexMarkup
{
    partial class ConfigRegexMarkup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            instance = null;
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigRegexMarkup));
            this.openFileDialogRegex = new System.Windows.Forms.OpenFileDialog();
            this.labelLanguage = new System.Windows.Forms.Label();
            this.comboBoxLang = new System.Windows.Forms.ComboBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelExternalFile = new System.Windows.Forms.Label();
            this.checkBoxExternalFile = new System.Windows.Forms.CheckBox();
            this.textBoxExternalFile = new System.Windows.Forms.TextBox();
            this.buttonExaminar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialogRegex
            // 
            this.openFileDialogRegex.FileName = "regex.xml";
            this.openFileDialogRegex.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialogRegex_FileOk);
            // 
            // labelLanguage
            // 
            this.labelLanguage.AutoSize = true;
            this.labelLanguage.Location = new System.Drawing.Point(71, 36);
            this.labelLanguage.Name = "labelLanguage";
            this.labelLanguage.Size = new System.Drawing.Size(41, 13);
            this.labelLanguage.TabIndex = 0;
            this.labelLanguage.Text = "Idioma:";
            // 
            // comboBoxLang
            // 
            this.comboBoxLang.FormattingEnabled = true;
            this.comboBoxLang.Location = new System.Drawing.Point(118, 33);
            this.comboBoxLang.Name = "comboBoxLang";
            this.comboBoxLang.Size = new System.Drawing.Size(222, 21);
            this.comboBoxLang.TabIndex = 1;
            // 
            // buttonSave
            // 
            this.buttonSave.AutoSize = true;
            this.buttonSave.Location = new System.Drawing.Point(170, 124);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Guardar";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelExternalFile
            // 
            this.labelExternalFile.AutoSize = true;
            this.labelExternalFile.Location = new System.Drawing.Point(4, 71);
            this.labelExternalFile.Name = "labelExternalFile";
            this.labelExternalFile.Size = new System.Drawing.Size(108, 13);
            this.labelExternalFile.TabIndex = 3;
            this.labelExternalFile.Text = "Usar archivo externo:";
            // 
            // checkBoxExternalFile
            // 
            this.checkBoxExternalFile.AutoSize = true;
            this.checkBoxExternalFile.Location = new System.Drawing.Point(118, 71);
            this.checkBoxExternalFile.Name = "checkBoxExternalFile";
            this.checkBoxExternalFile.Size = new System.Drawing.Size(15, 14);
            this.checkBoxExternalFile.TabIndex = 4;
            this.checkBoxExternalFile.UseVisualStyleBackColor = true;
            this.checkBoxExternalFile.CheckedChanged += new System.EventHandler(this.checkBoxExternalFile_CheckedChanged);
            // 
            // textBoxExternalFile
            // 
            this.textBoxExternalFile.Location = new System.Drawing.Point(140, 68);
            this.textBoxExternalFile.Name = "textBoxExternalFile";
            this.textBoxExternalFile.Size = new System.Drawing.Size(200, 20);
            this.textBoxExternalFile.TabIndex = 5;
            // 
            // buttonExaminar
            // 
            this.buttonExaminar.Location = new System.Drawing.Point(352, 66);
            this.buttonExaminar.Name = "buttonExaminar";
            this.buttonExaminar.Size = new System.Drawing.Size(69, 23);
            this.buttonExaminar.TabIndex = 6;
            this.buttonExaminar.Text = "Examinar...";
            this.buttonExaminar.UseVisualStyleBackColor = true;
            this.buttonExaminar.Click += new System.EventHandler(this.buttonExaminar_Click);
            // 
            // ConfigRegexMarkup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 163);
            this.Controls.Add(this.buttonExaminar);
            this.Controls.Add(this.textBoxExternalFile);
            this.Controls.Add(this.checkBoxExternalFile);
            this.Controls.Add(this.labelExternalFile);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.comboBoxLang);
            this.Controls.Add(this.labelLanguage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigRegexMarkup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "configRegexMarkup";
            this.Load += new System.EventHandler(this.ConfigRegexMarkup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialogRegex;
        private System.Windows.Forms.Label labelLanguage;
        private System.Windows.Forms.ComboBox comboBoxLang;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelExternalFile;
        private System.Windows.Forms.CheckBox checkBoxExternalFile;
        private System.Windows.Forms.TextBox textBoxExternalFile;
        private System.Windows.Forms.Button buttonExaminar;
    }
}