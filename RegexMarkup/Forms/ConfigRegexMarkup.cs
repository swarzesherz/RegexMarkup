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
using System.Runtime.InteropServices;

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
        private BindingSource comboBoxLangDS = null;
        public ConfigRegexMarkup()
        {
            InitializeComponent();
            /* Diccionario con los idiomas disponibles */
            this.languages = new Dictionary<string, string>();
            this.languages.Add("es-ES", Resources.configRegexMarkup_esES);
            this.languages.Add("en-US", Resources.configRegexMarkup_enUS);
            this.comboBoxLangDS = new BindingSource(this.languages, null);
            /* Agregando los datos del diccionadio al comboBox */
            this.comboBoxLang.BindingContext = new BindingContext();
            this.comboBoxLang.DisplayMember = "Value";
            this.comboBoxLang.ValueMember = "Key";
            this.comboBoxLang.DataSource = this.comboBoxLangDS;
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
            if (this.comboBoxLang.SelectedValue != null)
            {
                Settings.Default.language = this.comboBoxLang.SelectedValue.ToString();
                Resources.Culture = new CultureInfo(Settings.Default.language);
            }
            
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();
            this.Close();
            this.Dispose();
        }

        #region Disable close button
        /// <summary>
        /// Sección de código para quitar el boron "x"
        /// </summary>
        const int MF_BYPOSITION = 0x400;
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);

        private void ConfigRegexMarkup_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);

            int menuItemCount = GetMenuItemCount(hMenu);
            /* Quitando boton cerrar "x" */
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
        }
        #endregion
    }
}
