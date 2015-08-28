namespace RegexMarkup.Forms
{
    partial class EditAttribute
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditAttribute));
            this.groupBoxTag = new System.Windows.Forms.GroupBox();
            this.richTextBoxTag = new System.Windows.Forms.RichTextBox();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxTools = new System.Windows.Forms.GroupBox();
            this.toolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxTag.SuspendLayout();
            this.groupBoxTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTag
            // 
            this.groupBoxTag.Controls.Add(this.richTextBoxTag);
            this.groupBoxTag.Location = new System.Drawing.Point(10, 49);
            this.groupBoxTag.Name = "groupBoxTag";
            this.groupBoxTag.Size = new System.Drawing.Size(580, 100);
            this.groupBoxTag.TabIndex = 0;
            this.groupBoxTag.TabStop = false;
            this.groupBoxTag.Text = "TagName";
            // 
            // richTextBoxTag
            // 
            this.richTextBoxTag.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxTag.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxTag.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxTag.Name = "richTextBoxTag";
            this.richTextBoxTag.ReadOnly = true;
            this.richTextBoxTag.Size = new System.Drawing.Size(574, 81);
            this.richTextBoxTag.TabIndex = 0;
            this.richTextBoxTag.Text = "";
            // 
            // buttonEdit
            // 
            this.buttonEdit.FlatAppearance.BorderSize = 0;
            this.buttonEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEdit.Image = global::RegexMarkup.Properties.Resources.edit;
            this.buttonEdit.Location = new System.Drawing.Point(10, 15);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(23, 23);
            this.buttonEdit.TabIndex = 1;
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Image = global::RegexMarkup.Properties.Resources.cancel;
            this.buttonCancel.Location = new System.Drawing.Point(39, 15);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(23, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBoxTools
            // 
            this.groupBoxTools.Controls.Add(this.buttonEdit);
            this.groupBoxTools.Controls.Add(this.buttonCancel);
            this.groupBoxTools.Location = new System.Drawing.Point(10, 2);
            this.groupBoxTools.Name = "groupBoxTools";
            this.groupBoxTools.Size = new System.Drawing.Size(85, 45);
            this.groupBoxTools.TabIndex = 3;
            this.groupBoxTools.TabStop = false;
            this.groupBoxTools.Text = "Herramientas";
            // 
            // EditAttribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(598, 262);
            this.Controls.Add(this.groupBoxTools);
            this.Controls.Add(this.groupBoxTag);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditAttribute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EditAttribute";
            this.Load += new System.EventHandler(this.EditAttribute_Load);
            this.groupBoxTag.ResumeLayout(false);
            this.groupBoxTools.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTag;
        private System.Windows.Forms.RichTextBox richTextBoxTag;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxTools;
        private System.Windows.Forms.ToolTip toolTipInfo;
    }
}