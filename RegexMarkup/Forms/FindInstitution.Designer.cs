namespace RegexMarkup.Forms
{
    partial class FindInstitution
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindInstitution));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.submitSearch = new System.Windows.Forms.Button();
            this.textSearch = new System.Windows.Forms.TextBox();
            this.institutionLabel = new System.Windows.Forms.Label();
            this.countryLabel = new System.Windows.Forms.Label();
            this.countryCbox = new System.Windows.Forms.ComboBox();
            this.groupBoxTools = new System.Windows.Forms.GroupBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonInsert = new System.Windows.Forms.Button();
            this.buttonCopy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBoxTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 94);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(792, 272);
            this.dataGridView1.TabIndex = 0;
            // 
            // submitSearch
            // 
            this.submitSearch.Location = new System.Drawing.Point(534, 58);
            this.submitSearch.Name = "submitSearch";
            this.submitSearch.Size = new System.Drawing.Size(75, 23);
            this.submitSearch.TabIndex = 1;
            this.submitSearch.Text = "Buscar";
            this.submitSearch.UseVisualStyleBackColor = true;
            this.submitSearch.Click += new System.EventHandler(this.submitSearch_Click);
            // 
            // textSearch
            // 
            this.textSearch.Location = new System.Drawing.Point(73, 61);
            this.textSearch.Name = "textSearch";
            this.textSearch.Size = new System.Drawing.Size(292, 20);
            this.textSearch.TabIndex = 2;
            // 
            // institutionLabel
            // 
            this.institutionLabel.AutoSize = true;
            this.institutionLabel.Location = new System.Drawing.Point(10, 64);
            this.institutionLabel.Name = "institutionLabel";
            this.institutionLabel.Size = new System.Drawing.Size(55, 13);
            this.institutionLabel.TabIndex = 3;
            this.institutionLabel.Text = "Institución";
            // 
            // countryLabel
            // 
            this.countryLabel.AutoSize = true;
            this.countryLabel.Location = new System.Drawing.Point(372, 64);
            this.countryLabel.Name = "countryLabel";
            this.countryLabel.Size = new System.Drawing.Size(29, 13);
            this.countryLabel.TabIndex = 4;
            this.countryLabel.Text = "País";
            // 
            // countryCbox
            // 
            this.countryCbox.FormattingEnabled = true;
            this.countryCbox.Location = new System.Drawing.Point(407, 60);
            this.countryCbox.Name = "countryCbox";
            this.countryCbox.Size = new System.Drawing.Size(121, 21);
            this.countryCbox.TabIndex = 5;
            // 
            // groupBoxTools
            // 
            this.groupBoxTools.Controls.Add(this.buttonCancel);
            this.groupBoxTools.Controls.Add(this.buttonInsert);
            this.groupBoxTools.Controls.Add(this.buttonCopy);
            this.groupBoxTools.Location = new System.Drawing.Point(10, 2);
            this.groupBoxTools.Name = "groupBoxTools";
            this.groupBoxTools.Size = new System.Drawing.Size(770, 45);
            this.groupBoxTools.TabIndex = 18;
            this.groupBoxTools.TabStop = false;
            this.groupBoxTools.Text = "Herramientas";
            // 
            // buttonCancel
            // 
            this.buttonCancel.AutoSize = true;
            this.buttonCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Image = ((System.Drawing.Image)(resources.GetObject("buttonCancel.Image")));
            this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCancel.Location = new System.Drawing.Point(150, 15);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(73, 23);
            this.buttonCancel.TabIndex = 20;
            this.buttonCancel.Text = "Cancelar";
            this.buttonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonInsert
            // 
            this.buttonInsert.AutoSize = true;
            this.buttonInsert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonInsert.FlatAppearance.BorderSize = 0;
            this.buttonInsert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonInsert.Image = global::RegexMarkup.Properties.Resources.code;
            this.buttonInsert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonInsert.Location = new System.Drawing.Point(10, 15);
            this.buttonInsert.Name = "buttonInsert";
            this.buttonInsert.Size = new System.Drawing.Size(65, 23);
            this.buttonInsert.TabIndex = 14;
            this.buttonInsert.Text = "Insertar";
            this.buttonInsert.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonInsert.UseVisualStyleBackColor = true;
            this.buttonInsert.Click += new System.EventHandler(this.buttonInsert_Click);
            // 
            // buttonCopy
            // 
            this.buttonCopy.AutoSize = true;
            this.buttonCopy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonCopy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCopy.FlatAppearance.BorderSize = 0;
            this.buttonCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCopy.Image = global::RegexMarkup.Properties.Resources.copy;
            this.buttonCopy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCopy.Location = new System.Drawing.Point(81, 15);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(63, 23);
            this.buttonCopy.TabIndex = 0;
            this.buttonCopy.Text = "Copiar";
            this.buttonCopy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // FindInstitution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 366);
            this.Controls.Add(this.groupBoxTools);
            this.Controls.Add(this.countryCbox);
            this.Controls.Add(this.countryLabel);
            this.Controls.Add(this.institutionLabel);
            this.Controls.Add(this.textSearch);
            this.Controls.Add(this.submitSearch);
            this.Controls.Add(this.dataGridView1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindInstitution";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FindInstitution";
            this.Load += new System.EventHandler(this.FindInstitution_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBoxTools.ResumeLayout(false);
            this.groupBoxTools.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button submitSearch;
        private System.Windows.Forms.TextBox textSearch;
        private System.Windows.Forms.Label institutionLabel;
        private System.Windows.Forms.Label countryLabel;
        private System.Windows.Forms.ComboBox countryCbox;
        private System.Windows.Forms.GroupBox groupBoxTools;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonCopy;
        private System.Windows.Forms.Button buttonInsert;
    }
}