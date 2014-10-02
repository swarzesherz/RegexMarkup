using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegexMarkup.Forms
{
    public partial class InstitutionParams : Form
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static InstitutionParams instance = null;
        static readonly object padlock = new object();
        private String originalAffiliation = null;

        public static InstitutionParams Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new InstitutionParams();
                    }
                    return instance;
                }
            }
        }
        #endregion

        public String OriginalAffiliation
        {
            get { return originalAffiliation; }
            set { originalAffiliation = value; }
        }

        public String Id
        {
            get { return textBoxId.Text; }
        }

        public String OrgDiv1
        {
            get { return textBoxOrgDiv1.Text; }
        }

        public String OrgDiv2
        {
            get { return textBoxOrgDiv2.Text; }
        }

        public String OrgDiv3
        {
            get { return textBoxOrgDiv3.Text; }
        }

        public String ZipCode
        {
            get { return textBoxZipCode.Text; }
        }

        public String Email
        {
            get { return textBoxEmail.Text; }
        }
        
        InstitutionParams()
        {
            InitializeComponent();
            this.richTextBoxOriginalText.HideSelection = false;
        }

        private void InstitutionParams_Load(object sender, EventArgs e)
        {
            this.textBoxId.Clear();
            this.textBoxOrgDiv1.Clear();
            this.textBoxOrgDiv2.Clear();
            this.textBoxOrgDiv3.Clear();
            this.textBoxZipCode.Clear();
            this.textBoxEmail.Clear();
            this.richTextBoxOriginalText.Text = this.OriginalAffiliation;
        }

        private void buttonCopyOrgDiv1_Click(object sender, EventArgs e)
        {
            this.textBoxOrgDiv1.Text = this.richTextBoxOriginalText.SelectedText.Trim();
        }

        private void buttonCopyOrgDiv2_Click(object sender, EventArgs e)
        {
            this.textBoxOrgDiv2.Text = this.richTextBoxOriginalText.SelectedText.Trim();
        }

        private void buttonCopyOrgDiv3_Click(object sender, EventArgs e)
        {
            this.textBoxOrgDiv3.Text = this.richTextBoxOriginalText.SelectedText.Trim();
        }

        private void buttonCopyZipCode_Click(object sender, EventArgs e)
        {
            this.textBoxZipCode.Text = this.richTextBoxOriginalText.SelectedText.Trim();
        }

        private void buttonCopyEmail_Click(object sender, EventArgs e)
        {
            this.textBoxEmail.Text = this.richTextBoxOriginalText.SelectedText.Trim();
        }

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
