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
            this.buttonRedo = new System.Windows.Forms.Button();
            this.buttonUndo = new System.Windows.Forms.Button();
            this.buttonEditAttr = new System.Windows.Forms.Button();
            this.buttonClearTag = new System.Windows.Forms.Button();
            this.groupBoxOriginal = new System.Windows.Forms.GroupBox();
            this.groupBoxMarkup = new System.Windows.Forms.GroupBox();
            this.groupBoxTools.SuspendLayout();
            this.groupBoxOriginal.SuspendLayout();
            this.groupBoxMarkup.SuspendLayout();
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
            this.buttonFirst.Location = new System.Drawing.Point(10, 418);
            this.buttonFirst.Name = "buttonFirst";
            this.buttonFirst.Size = new System.Drawing.Size(75, 23);
            this.buttonFirst.TabIndex = 2;
            this.buttonFirst.Text = "Primera";
            this.buttonFirst.UseVisualStyleBackColor = true;
            this.buttonFirst.Click += new System.EventHandler(this.buttonFirst_Click);
            // 
            // buttonPrev
            // 
            this.buttonPrev.Location = new System.Drawing.Point(91, 418);
            this.buttonPrev.Name = "buttonPrev";
            this.buttonPrev.Size = new System.Drawing.Size(75, 23);
            this.buttonPrev.TabIndex = 3;
            this.buttonPrev.Text = "Anterior";
            this.buttonPrev.UseVisualStyleBackColor = true;
            this.buttonPrev.Click += new System.EventHandler(this.buttonPrev_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(172, 418);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 4;
            this.buttonNext.Text = "Siguiente";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonLast
            // 
            this.buttonLast.Location = new System.Drawing.Point(253, 418);
            this.buttonLast.Name = "buttonLast";
            this.buttonLast.Size = new System.Drawing.Size(75, 23);
            this.buttonLast.TabIndex = 5;
            this.buttonLast.Text = "Última";
            this.buttonLast.UseVisualStyleBackColor = true;
            this.buttonLast.Click += new System.EventHandler(this.buttonLast_Click);
            // 
            // radioButtonYes
            // 
            this.radioButtonYes.AutoSize = true;
            this.radioButtonYes.Location = new System.Drawing.Point(403, 421);
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
            this.radioButtonNo.Location = new System.Drawing.Point(443, 421);
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
            this.labelCitationStatus.Location = new System.Drawing.Point(334, 423);
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
            this.buttonEnd.Location = new System.Drawing.Point(699, 418);
            this.buttonEnd.Name = "buttonEnd";
            this.buttonEnd.Size = new System.Drawing.Size(75, 25);
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
            this.citationOf.Location = new System.Drawing.Point(696, 0);
            this.citationOf.Name = "citationOf";
            this.citationOf.Padding = new System.Windows.Forms.Padding(0, 3, 12, 0);
            this.citationOf.Size = new System.Drawing.Size(87, 18);
            this.citationOf.TabIndex = 16;
            this.citationOf.Text = "Cita x de n";
            // 
            // groupBoxTools
            // 
            this.groupBoxTools.Controls.Add(this.buttonRedo);
            this.groupBoxTools.Controls.Add(this.buttonUndo);
            this.groupBoxTools.Controls.Add(this.buttonEditAttr);
            this.groupBoxTools.Controls.Add(this.buttonClearTag);
            this.groupBoxTools.Location = new System.Drawing.Point(10, 2);
            this.groupBoxTools.Name = "groupBoxTools";
            this.groupBoxTools.Size = new System.Drawing.Size(126, 45);
            this.groupBoxTools.TabIndex = 17;
            this.groupBoxTools.TabStop = false;
            this.groupBoxTools.Text = "Herramientas";
            // 
            // buttonRedo
            // 
            this.buttonRedo.AutoSize = true;
            this.buttonRedo.FlatAppearance.BorderSize = 0;
            this.buttonRedo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRedo.Image = global::RegexMarkup.Properties.Resources.redo;
            this.buttonRedo.Location = new System.Drawing.Point(94, 15);
            this.buttonRedo.Name = "buttonRedo";
            this.buttonRedo.Size = new System.Drawing.Size(22, 22);
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
            this.buttonUndo.Location = new System.Drawing.Point(66, 15);
            this.buttonUndo.Name = "buttonUndo";
            this.buttonUndo.Size = new System.Drawing.Size(22, 22);
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
            this.buttonEditAttr.Location = new System.Drawing.Point(38, 15);
            this.buttonEditAttr.Name = "buttonEditAttr";
            this.buttonEditAttr.Size = new System.Drawing.Size(22, 24);
            this.buttonEditAttr.TabIndex = 1;
            this.buttonEditAttr.UseVisualStyleBackColor = true;
            // 
            // buttonClearTag
            // 
            this.buttonClearTag.AutoSize = true;
            this.buttonClearTag.FlatAppearance.BorderSize = 0;
            this.buttonClearTag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClearTag.Image = global::RegexMarkup.Properties.Resources.trash;
            this.buttonClearTag.Location = new System.Drawing.Point(10, 15);
            this.buttonClearTag.Name = "buttonClearTag";
            this.buttonClearTag.Size = new System.Drawing.Size(22, 23);
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
            // ValidateMarkup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 449);
            this.Controls.Add(this.groupBoxMarkup);
            this.Controls.Add(this.groupBoxOriginal);
            this.Controls.Add(this.groupBoxTools);
            this.Controls.Add(this.citationOf);
            this.Controls.Add(this.buttonEnd);
            this.Controls.Add(this.labelCitationStatus);
            this.Controls.Add(this.radioButtonNo);
            this.Controls.Add(this.radioButtonYes);
            this.Controls.Add(this.buttonLast);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPrev);
            this.Controls.Add(this.buttonFirst);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxMarkup;
        private Button buttonFirst;
        private Button buttonPrev;
        private Button buttonNext;
        private Button buttonLast;
        private RadioButton radioButtonYes;
        private RadioButton radioButtonNo;
        private Label labelCitationStatus;
        private RichTextBox richTextBoxOriginal;
        private Button buttonEnd;
        private Label citationOf;
        private ToolTip toolTipInfo;
        private GroupBox groupBoxTools;
        private Button buttonClearTag;
        private Button buttonEditAttr;
        private GroupBox groupBoxOriginal;
        private GroupBox groupBoxMarkup;
        private Button buttonUndo;
        private Button buttonRedo;
    }
}