using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RegexMarkup.Properties;

namespace RegexMarkup.Forms
{
    public partial class Waiting : Form
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static Waiting instance = null;
        static readonly object padlock = new object();

        public static Waiting Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Waiting();
                    }
                    return instance;
                }
            }
        }
        #endregion
        public Waiting()
        {
            InitializeComponent();
            /* Textos del formulario */
            this.Text = Resources.Waiting_labelWait;
            this.labelWait.Text = Resources.Waiting_labelWait;
        }
    }
}
