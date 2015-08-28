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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValidateMarkup));
            this.richTextBoxMarkup = new System.Windows.Forms.RichTextBox();
            this.buttonFirst = new System.Windows.Forms.Button();
            this.buttonPrev = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonLast = new System.Windows.Forms.Button();
            this.radioButtonYes = new System.Windows.Forms.RadioButton();
            this.radioButtonNo = new System.Windows.Forms.RadioButton();
            this.labelCitationStatus = new System.Windows.Forms.Label();
            this.richTextBoxOriginal = new System.Windows.Forms.RichTextBox();
            this.buttonEnd = new System.Windows.Forms.Button();
            this.citationOf = new System.Windows.Forms.Label();
            this.toolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxTools = new System.Windows.Forms.GroupBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonRedo = new System.Windows.Forms.Button();
            this.buttonUndo = new System.Windows.Forms.Button();
            this.buttonEditAttr = new System.Windows.Forms.Button();
            this.buttonClearTag = new System.Windows.Forms.Button();
            this.groupBoxOriginal = new System.Windows.Forms.GroupBox();
            this.groupBoxMarkup = new System.Windows.Forms.GroupBox();
            this.panelNavigation = new System.Windows.Forms.Panel();
            this.groupBoxTools.SuspendLayout();
            this.groupBoxOriginal.SuspendLayout();
            this.groupBoxMarkup.SuspendLayout();
            this.panelNavigation.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxMarkup
            // 
            this.richTextBoxMarkup.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxMarkup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMarkup.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxMarkup.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxMarkup.Name = "richTextBoxMarkup";
            this.richTextBoxMarkup.Size = new System.Drawing.Size(759, 165);
            this.richTextBoxMarkup.TabIndex = 1;
            this.richTextBoxMarkup.Text = "";
            // 
            // buttonFirst
            // 
            this.buttonFirst.FlatAppearance.BorderSize = 0;
            this.buttonFirst.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFirst.Image = global::RegexMarkup.Properties.Resources.first;
            this.buttonFirst.Location = new System.Drawing.Point(10, 9);
            this.buttonFirst.Name = "buttonFirst";
            this.buttonFirst.Size = new System.Drawing.Size(23, 23);
            this.buttonFirst.TabIndex = 2;
            this.buttonFirst.UseVisualStyleBackColor = true;
            this.buttonFirst.Click += new System.EventHandler(this.buttonFirst_Click);
            // 
            // buttonPrev
            // 
            this.buttonPrev.FlatAppearance.BorderSize = 0;
            this.buttonPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPrev.Image = global::RegexMarkup.Properties.Resources.prev;
            this.buttonPrev.Location = new System.Drawing.Point(44, 9);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(23, 23);
            this.buttonPrev.TabIndex = 3;
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.FlatAppearance.BorderSize = 0;
            this.buttonNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNext.Image = global::RegexMarkup.Properties.Resources.next;
            this.buttonNext.Location = new System.Drawing.Point(172, 9);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(23, 23);
            this.buttonNext.TabIndex = 4;
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonLast
            // 
            this.buttonLast.FlatAppearance.BorderSize = 0;
            this.buttonLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLast.Image = global::RegexMarkup.Properties.Resources.last;
            this.buttonLast.Location = new System.Drawing.Point(206, 9);
            this.buttonLast.Name = "buttonLast";
            this.buttonLast.Size = new System.Drawing.Size(23, 23);
            this.buttonLast.TabIndex = 5;
            this.buttonLast.UseVisualStyleBackColor = true;
            this.buttonLast.Click += new System.EventHandler(this.buttonLast_Click);
            // 
            // radioButtonYes
            // 
            this.radioButtonYes.AutoSize = true;
            this.radioButtonYes.Location = new System.Drawing.Point(283, 21);
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
            this.radioButtonNo.Location = new System.Drawing.Point(323, 21);
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
            this.labelCitationStatus.Location = new System.Drawing.Point(214, 23);
            this.labelCitationStatus.Name = "labelCitationStatus";
            this.labelCitationStatus.Size = new System.Drawing.Size(63, 13);
            this.labelCitationStatus.TabIndex = 11;
            this.labelCitationStatus.Text = "Marcar cita:";
            // 
            // richTextBoxOriginal
            // 
            this.richTextBoxOriginal.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxOriginal.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxOriginal.Location = new System.Drawing.Point(3, 16);
            this.richTextBoxOriginal.Name = "richTextBoxOriginal";
            this.richTextBoxOriginal.ReadOnly = true;
            this.richTextBoxOriginal.Size = new System.Drawing.Size(759, 113);
            this.richTextBoxOriginal.TabIndex = 13;
            this.richTextBoxOriginal.Tag = "";
            this.richTextBoxOriginal.Text = "";
            // 
            // buttonEnd
            // 
            this.buttonEnd.AutoSize = true;
            this.buttonEnd.FlatAppearance.BorderSize = 0;
            this.buttonEnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEnd.Image = global::RegexMarkup.Properties.Resources.save;
            this.buttonEnd.Location = new System.Drawing.Point(10, 15);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(23, 23);
            this.buttonEnd.TabIndex = 14;
            this.buttonEnd.UseVisualStyleBackColor = true;
            this.buttonEnd.Click += new System.EventHandler(this.buttonEnd_Click);
            // 
            // citationOf
            // 
            this.citationOf.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.citationOf.Location = new System.Drawing.Point(72, 12);
            this.citationOf.Name = "citationOf";
            this.citationOf.Size = new System.Drawing.Size(100, 18);
            this.citationOf.TabIndex = 16;
            this.citationOf.Text = "x de n";
            this.citationOf.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxTools
            // 
            this.groupBoxTools.Controls.Add(this.buttonCancel);
            this.groupBoxTools.Controls.Add(this.buttonRedo);
            this.groupBoxTools.Controls.Add(this.buttonUndo);
            this.groupBoxTools.Controls.Add(this.buttonEditAttr);
            this.groupBoxTools.Controls.Add(this.buttonClearTag);
            this.groupBoxTools.Controls.Add(this.buttonEnd);
            this.groupBoxTools.Location = new System.Drawing.Point(10, 2);
            this.groupBoxTools.Name = "groupBoxTools";
            this.groupBoxTools.Size = new System.Drawing.Size(198, 45);
            this.groupBoxTools.TabIndex = 17;
            this.groupBoxTools.TabStop = false;
            this.groupBoxTools.Text = "Herramientas";
            // 
            // buttonCancel
            // 
            this.buttonCancel.AutoSize = true;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Image = global::RegexMarkup.Properties.Resources.cancel;
            this.buttonCancel.Location = new System.Drawing.Point(38, 15);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(23, 23);
            this.buttonCancel.TabIndex = 20;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonRedo
            // 
            this.buttonRedo.AutoSize = true;
            this.buttonRedo.FlatAppearance.BorderSize = 0;
            this.buttonRedo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRedo.Image = global::RegexMarkup.Properties.Resources.redo;
            this.buttonRedo.Location = new System.Drawing.Point(164, 15);
            this.buttonRedo.Name = "buttonRedo";
            this.buttonRedo.Size = new System.Drawing.Size(23, 23);
            this.buttonRedo.TabIndex = 3;
            this.buttonRedo.UseVisualStyleBackColor = true;
            this.buttonRedo.Click += new System.EventHandler(this.buttonRedo_Click);
            // 
            // buttonUndo
            // 
            this.buttonUndo.AutoSize = true;
            this.buttonUndo.FlatAppearance.BorderSize = 0;
            this.buttonUndo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUndo.Image = global::RegexMarkup.Properties.Resources.undo;
            this.buttonUndo.Location = new System.Drawing.Point(136, 15);
            this.buttonUndo.Name = "buttonUndo";
            this.buttonUndo.Size = new System.Drawing.Size(23, 23);
            this.buttonUndo.TabIndex = 2;
            this.buttonUndo.UseVisualStyleBackColor = true;
            this.buttonUndo.Click += new System.EventHandler(this.buttonUndo_Click);
            // 
            // buttonEditAttr
            // 
            this.buttonEditAttr.AutoSize = true;
            this.buttonEditAttr.FlatAppearance.BorderSize = 0;
            this.buttonEditAttr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEditAttr.Image = global::RegexMarkup.Properties.Resources.pencil;
            this.buttonEditAttr.Location = new System.Drawing.Point(108, 15);
            this.buttonEditAttr.Name = "buttonEditAttr";
            this.buttonEditAttr.Size = new System.Drawing.Size(23, 23);
            this.buttonEditAttr.TabIndex = 1;
            this.buttonEditAttr.UseVisualStyleBackColor = true;
            this.buttonEditAttr.Click += new System.EventHandler(this.buttonEditAttr_Click);
            // 
            // buttonClearTag
            // 
            this.buttonClearTag.AutoSize = true;
            this.buttonClearTag.FlatAppearance.BorderSize = 0;
            this.buttonClearTag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearTag.Image = global::RegexMarkup.Properties.Resources.trash;
            this.buttonClearTag.Location = new System.Drawing.Point(80, 15);
            this.buttonClearTag.Name = "buttonClearTag";
            this.buttonClearTag.Size = new System.Drawing.Size(23, 23);
            this.buttonClearTag.TabIndex = 0;
            this.buttonClearTag.UseVisualStyleBackColor = true;
            this.buttonClearTag.Click += new System.EventHandler(this.buttonClearTag_Click);
            // 
            // groupBoxOriginal
            // 
            this.groupBoxOriginal.Controls.Add(this.richTextBoxOriginal);
            this.groupBoxOriginal.Location = new System.Drawing.Point(10, 282);
            this.groupBoxOriginal.Name = "groupBoxOriginal";
            this.groupBoxOriginal.Size = new System.Drawing.Size(765, 132);
            this.groupBoxOriginal.TabIndex = 18;
            this.groupBoxOriginal.TabStop = false;
            this.groupBoxOriginal.Text = "Cita original";
            // 
            // groupBoxMarkup
            // 
            this.groupBoxMarkup.Controls.Add(this.richTextBoxMarkup);
            this.groupBoxMarkup.Location = new System.Drawing.Point(10, 96);
            this.groupBoxMarkup.Name = "groupBoxMarkup";
            this.groupBoxMarkup.Size = new System.Drawing.Size(765, 184);
            this.groupBoxMarkup.TabIndex = 19;
            this.groupBoxMarkup.TabStop = false;
            this.groupBoxMarkup.Text = "Cita marcada";
            // 
            // panelNavigation
            // 
            this.panelNavigation.Controls.Add(this.buttonFirst);
            this.panelNavigation.Controls.Add(this.buttonPrev);
            this.panelNavigation.Controls.Add(this.citationOf);
            this.panelNavigation.Controls.Add(this.buttonNext);
            this.panelNavigation.Controls.Add(this.buttonLast);
            this.panelNavigation.Location = new System.Drawing.Point(278, 416);
            this.panelNavigation.Name = "panelNavigation";
            this.panelNavigation.Size = new System.Drawing.Size(244, 40);
            this.panelNavigation.TabIndex = 22;
            // 
            // ValidateMarkup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.panelNavigation);
            this.Controls.Add(this.groupBoxMarkup);
            this.Controls.Add(this.groupBoxOriginal);
            this.Controls.Add(this.groupBoxTools);
            this.Controls.Add(this.labelCitationStatus);
            this.Controls.Add(this.radioButtonNo);
            this.Controls.Add(this.radioButtonYes);
            this.Icon = System.Drawing.Icon.FromHandle(global::RegexMarkup.Properties.Resources.regex.GetHicon());
            this.MinimizeBox = false;
            this.Name = "ValidateMarkup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ValidateMarkup";
            this.Load += new System.EventHandler(this.ValidateMarkup_Load);
            this.groupBoxTools.ResumeLayout(false);
            this.groupBoxTools.PerformLayout();
            this.groupBoxOriginal.ResumeLayout(false);
            this.groupBoxMarkup.ResumeLayout(false);
            this.panelNavigation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxMarkup;
        private System.Windows.Forms.Button buttonFirst;
        private System.Windows.Forms.Button buttonPrev;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonLast;
        private System.Windows.Forms.RadioButton radioButtonYes;
        private System.Windows.Forms.RadioButton radioButtonNo;
        private System.Windows.Forms.Label labelCitationStatus;
        private System.Windows.Forms.RichTextBox richTextBoxOriginal;
        private System.Windows.Forms.Button buttonEnd;
        private System.Windows.Forms.Label citationOf;
        private System.Windows.Forms.ToolTip toolTipInfo;
        private System.Windows.Forms.GroupBox groupBoxTools;
        private System.Windows.Forms.Button buttonClearTag;
        private System.Windows.Forms.Button buttonEditAttr;
        private System.Windows.Forms.GroupBox groupBoxOriginal;
        private System.Windows.Forms.GroupBox groupBoxMarkup;
        private System.Windows.Forms.Button buttonUndo;
        private System.Windows.Forms.Button buttonRedo;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panelNavigation;
        private Button nextMarkupButtons;
    }
}