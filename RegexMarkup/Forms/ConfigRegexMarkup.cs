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

        private Dictionary<String, String> languages = null;
        public ConfigRegexMarkup()
        {
            InitializeComponent();
            /* Diccionario con los idiomas disponibles */
            this.languages = new Dictionary<string, string>();
            languages.Add("es-ES", Resources.configRegexMarkup_esES);
            languages.Add("en-US", Resources.configRegexMarkup_enUS);
            /* Agregando los datos del diccionadio al comboBox */
            this.comboBoxLang.DataSource = new BindingSource(languages, null);
            this.comboBoxLang.DisplayMember = "Value";
            this.comboBoxLang.ValueMember = "Key";
            if (languages.ContainsKey(Settings.Default.language))
            {
                this.comboBoxLang.SelectedValue = Settings.Default.language;
            }
            this.comboBoxLang.SelectedValueChanged += new EventHandler(comboBoxLang_SelectedValueChanged);
            /* Textos del formulario */
            this.labelLanguage.Text = Resources.configRegexMarkup_language;
            this.Text = Resources.configRegexMarkup_title;
        }

        private void comboBoxLang_SelectedValueChanged(object sender, EventArgs e)
        {
            /* Gurdando el idioma seleccionado en la configuración y cambiando el idioma actual */
            Settings.Default.language = this.comboBoxLang.SelectedValue.ToString();
            Settings.Default.Save();
            Resources.Culture = new CultureInfo(Settings.Default.language);
            
        }

        private void ConfigRegexMarkup_Load(object sender, EventArgs e)
        {

        }
    }
}
