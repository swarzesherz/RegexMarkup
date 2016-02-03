namespace RegexMarkup.Forms
{
    partial class Waiting
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
            this.waitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // waitButton
            // 
            this.waitButton.FlatAppearance.BorderSize = 0;
            this.waitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.waitButton.Image = global::RegexMarkup.Properties.Resources.hourglass_start;
            this.waitButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.waitButton.Location = new System.Drawing.Point(12, 9);
            this.waitButton.Name = "waitButton";
            this.waitButton.Size = new System.Drawing.Size(115, 23);
            this.waitButton.TabIndex = 1;
            this.waitButton.Text = "Espere por favor";
            this.waitButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.waitButton.UseVisualStyleBackColor = true;
            // 
            // Waiting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(139, 44);
            this.Controls.Add(this.waitButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Waiting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Waiting";
            this.Load += new System.EventHandler(this.Waiting_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button waitButton;
    }
}