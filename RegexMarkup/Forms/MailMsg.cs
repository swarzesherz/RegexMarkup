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
    public partial class MailMsg : Form
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static MailMsg instance = null;
        static readonly object padlock = new object();

        public static MailMsg Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new MailMsg();
                    }
                    return instance;
                }
            }
        }
        #endregion
        
        public String mailMsg;

        MailMsg()
        {
            InitializeComponent();
            /* Icon */
            this.Icon = System.Drawing.Icon.FromHandle(global::RegexMarkup.Properties.Resources.mail.GetHicon());
        }

        public void clear() {
            this.richTextBoxDescription.Text = "";
            this.mailMsg = "";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            this.mailMsg = this.richTextBoxDescription.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
