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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditAttribute));
            this.groupBoxTag = new System.Windows.Forms.GroupBox();
            this.richTextBoxTag = new System.Windows.Forms.RichTextBox();
            this.groupBoxTag.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTag
            // 
            this.groupBoxTag.Controls.Add(this.richTextBoxTag);
            this.groupBoxTag.Location = new System.Drawing.Point(10, 2);
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
            // EditAttribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 262);
            this.Controls.Add(this.groupBoxTag);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditAttribute";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EditAttribute";
            this.Load += new System.EventHandler(this.EditAttribute_Load);
            this.groupBoxTag.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTag;
        private System.Windows.Forms.RichTextBox richTextBoxTag;
    }
}