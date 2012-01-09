using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RegexMarkup
{
    partial class ValidateMarkup
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
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValidateMarkup));
            this.richTextBoxMarkup = new System.Windows.Forms.RichTextBox();
            this.buttonFirst = new System.Windows.Forms.Button();
            this.buttonPrev = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonLast = new System.Windows.Forms.Button();
            this.labelOriginal = new System.Windows.Forms.Label();
            this.labelMarkup = new System.Windows.Forms.Label();
            this.radioButtonYes = new System.Windows.Forms.RadioButton();
            this.radioButtonNo = new System.Windows.Forms.RadioButton();
            this.labelCitationStatus = new System.Windows.Forms.Label();
            this.richTextBoxOriginal = new System.Windows.Forms.RichTextBox();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.citationOf = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBoxMarkup
            // 
            this.richTextBoxMarkup.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxMarkup.Location = new System.Drawing.Point(12, 168);
            this.richTextBoxMarkup.Name = "richTextBoxMarkup";
            this.richTextBoxMarkup.Size = new System.Drawing.Size(690, 184);
            this.richTextBoxMarkup.TabIndex = 1;
            this.richTextBoxMarkup.Text = "";
            // 
            // buttonFirst
            // 
            this.buttonFirst.Location = new System.Drawing.Point(13, 383);
            this.buttonFirst.Name = "buttonFirst";
            this.buttonFirst.Size = new System.Drawing.Size(75, 23);
            this.buttonFirst.TabIndex = 2;
            this.buttonFirst.Text = "Primera";
            this.buttonFirst.UseVisualStyleBackColor = true;
            this.buttonFirst.Click += new System.EventHandler(this.buttonFirst_Click);
            // 
            // buttonPrev
            // 
            this.buttonPrev.Location = new System.Drawing.Point(94, 383);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(75, 23);
            this.buttonPrev.TabIndex = 3;
            this.buttonPrev.Text = "Anterior";
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(175, 383);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 4;
            this.buttonNext.Text = "Siguiente";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonLast
            // 
            this.buttonLast.Location = new System.Drawing.Point(256, 383);
            this.buttonLast.Name = "buttonLast";
            this.buttonLast.Size = new System.Drawing.Size(75, 23);
            this.buttonLast.TabIndex = 5;
            this.buttonLast.Text = "Última";
            this.buttonLast.UseVisualStyleBackColor = true;
            this.buttonLast.Click += new System.EventHandler(this.buttonLast_Click);
            // 
            // labelOriginal
            // 
            this.labelOriginal.AutoSize = true;
            this.labelOriginal.Location = new System.Drawing.Point(9, 7);
            this.labelOriginal.Name = "labelOriginal";
            this.labelOriginal.Size = new System.Drawing.Size(61, 13);
            this.labelOriginal.TabIndex = 7;
            this.labelOriginal.Text = "Cita original";
            // 
            // labelMarkup
            // 
            this.labelMarkup.AutoSize = true;
            this.labelMarkup.Location = new System.Drawing.Point(9, 148);
            this.labelMarkup.Name = "labelMarkup";
            this.labelMarkup.Size = new System.Drawing.Size(69, 13);
            this.labelMarkup.TabIndex = 8;
            this.labelMarkup.Text = "Cita marcada";
            // 
            // radioButtonYes
            // 
            this.radioButtonYes.AutoSize = true;
            this.radioButtonYes.Location = new System.Drawing.Point(406, 386);
            this.radioButtonYes.Name = "radioButtonYes";
            this.radioButtonYes.Size = new System.Drawing.Size(34, 17);
            this.radioButtonYes.TabIndex = 9;
            this.radioButtonYes.TabStop = true;
            this.radioButtonYes.Text = "Si";
            this.radioButtonYes.UseVisualStyleBackColor = true;
            // 
            // radioButtonNo
            // 
            this.radioButtonNo.AutoSize = true;
            this.radioButtonNo.Location = new System.Drawing.Point(446, 386);
            this.radioButtonNo.Name = "radioButtonNo";
            this.radioButtonNo.Size = new System.Drawing.Size(39, 17);
            this.radioButtonNo.TabIndex = 10;
            this.radioButtonNo.TabStop = true;
            this.radioButtonNo.Text = "No";
            this.radioButtonNo.UseVisualStyleBackColor = true;
            // 
            // labelCitationStatus
            // 
            this.labelCitationStatus.AutoSize = true;
            this.labelCitationStatus.Location = new System.Drawing.Point(337, 388);
            this.labelCitationStatus.Name = "labelCitationStatus";
            this.labelCitationStatus.Size = new System.Drawing.Size(63, 13);
            this.labelCitationStatus.TabIndex = 11;
            this.labelCitationStatus.Text = "Marcar cita:";
            // 
            // richTextBoxOriginal
            // 
            this.richTextBoxOriginal.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxOriginal.Location = new System.Drawing.Point(12, 25);
            this.richTextBoxOriginal.Name = "richTextBoxOriginal";
            this.richTextBoxOriginal.ReadOnly = true;
            this.richTextBoxOriginal.Size = new System.Drawing.Size(690, 113);
            this.richTextBoxOriginal.TabIndex = 13;
            this.richTextBoxOriginal.Text = "";
            // 
            // buttonEnd
            // 
            this.buttonEnd.AutoSize = true;
            this.buttonEnd.Location = new System.Drawing.Point(598, 380);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(75, 23);
            this.buttonEnd.TabIndex = 14;
            this.buttonEnd.Text = "Finalizar";
            this.buttonEnd.UseVisualStyleBackColor = true;
            this.buttonEnd.Click += new System.EventHandler(this.buttonEnd_Click);
            // 
            // citationOf
            // 
            this.citationOf.AutoSize = true;
            this.citationOf.Dock = System.Windows.Forms.DockStyle.Right;
            this.citationOf.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.citationOf.Location = new System.Drawing.Point(625, 0);
            this.citationOf.Name = "citationOf";
            this.citationOf.Padding = new System.Windows.Forms.Padding(0, 3, 12, 0);
            this.citationOf.Size = new System.Drawing.Size(87, 18);
            this.citationOf.TabIndex = 16;
            this.citationOf.Text = "Cita x de n";
            // 
            // ValidateMarkup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 418);
            this.Controls.Add(this.citationOf);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.richTextBoxOriginal);
            this.Controls.Add(this.labelCitationStatus);
            this.Controls.Add(this.radioButtonNo);
            this.Controls.Add(this.radioButtonYes);
            this.Controls.Add(this.labelMarkup);
            this.Controls.Add(this.labelOriginal);
            this.Controls.Add(this.buttonLast);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPrev);
            this.Controls.Add(this.buttonFirst);
            this.Controls.Add(this.richTextBoxMarkup);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "ValidateMarkup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ValidateMarkup";
            this.Load += new System.EventHandler(this.ValidateMarkup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxMarkup;
        private Button buttonFirst;
        private Button buttonPrev;
        private Button buttonNext;
        private Button buttonLast;
        private Label labelOriginal;
        private Label labelMarkup;
        private RadioButton radioButtonYes;
        private RadioButton radioButtonNo;
        private Label labelCitationStatus;
        private RichTextBox richTextBoxOriginal;
        private Button buttonEnd;
        private Label citationOf;
    }
}