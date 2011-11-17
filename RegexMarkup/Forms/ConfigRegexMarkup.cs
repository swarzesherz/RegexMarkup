using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using RegexMarkup.Properties;
using System.Globalization;

namespace RegexMarkup
{
    public partial class ConfigRegexMarkup : Form
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static ConfigRegexMarkup instance = null;
        static readonly object padlock = new object();

        public ConfigRegexMarkup()
        {
            InitializeComponent();
            /* Diccionario con los idiomas disponibles */
            Dictionary<String, String> languages = new Dictionary<string,string>();
            languages.Add("es-ES", "Español");
            languages.Add("en-US", "Ingles");
            this.comboBoxLang.DataSource = new BindingSource(languages, null);
            this.comboBoxLang.DisplayMember = "Value";
            this.comboBoxLang.ValueMember = "Key";
            this.comboBoxLang.SelectedValueChanged += new EventHandler(comboBoxLang_SelectedValueChanged);
        }

        public static ConfigRegexMarkup Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance==null)
                    {
                        instance = new ConfigRegexMarkup();
                    }
                    return instance;
                }
            }
        }
        #endregion

        private void comboBoxLang_SelectedValueChanged(object sender, EventArgs e)
        {
            Settings.Default.lang = this.comboBoxLang.SelectedValue.ToString();
            Resources.Culture = new CultureInfo(Settings.Default.lang);
        }
    }
}
