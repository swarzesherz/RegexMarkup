using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using RegexMarkup.Properties;


namespace RegexMarkup
{
    public partial class ValidateMarkup : Form
    {
        const int MF_BYPOSITION = 0x400;
        private List<markupStruct> citas;
        private CurrencyManager currencyManager = null;
        private XmlNode structNode = null;
        private int startColor = -1;
        public ValidateMarkup(ref List<markupStruct> citas, ref XmlNode structNode)
        {
            this.citas = citas;
            this.structNode = structNode;
            InitializeComponent();
            /* Agregando evento para cambiar el tamaño de los richtextbox cuando cambie el tamaño del formulario */
            this.SizeChanged += new EventHandler(ValidateMarkup_SizeChanged);
            this.currencyManager = (CurrencyManager)this.BindingContext[this.citas];
            this.richTextBoxOriginal.DataBindings.Add("Text", this.citas, "OriginalStr");
            this.richTextBoxMarkup.DataBindings.Add("Rtf", this.citas, "MarkedStrRtf");
            this.radioButtonYes.DataBindings.Add("Checked", this.citas, "Marked", false, DataSourceUpdateMode.OnPropertyChanged);
            this.radioButtonNo.DataBindings.Add("Checked", this.citas, "MarkedNo", false, DataSourceUpdateMode.OnPropertyChanged);
            /* Evento para colorear en la primera llamada */
            this.richTextBoxMarkup.BindingContextChanged += new EventHandler(this.currencyManager_PositionChanged);
            /* Evento para colorear cuando cambie la posicion */
            this.currencyManager.PositionChanged += new EventHandler(this.currencyManager_PositionChanged);
            /* Textos del formulario */
            this.Text = Resources.ValidateMarkup_title;
            this.labelOriginal.Text = Resources.ValidateMarkup_labelOriginal;
            this.labelMarkup.Text = Resources.ValidateMarkup_labelmarkup;
            this.buttonFirst.Text = Resources.ValidateMarkup_buttonFirst;
            this.buttonLast.Text = Resources.ValidateMarkup_buttonLast;
            this.buttonPrev.Text = Resources.ValidateMarkup_buttonPrev;
            this.buttonNext.Text = Resources.ValidateMarkup_buttonNext;
            this.buttonEnd.Text = Resources.ValidateMarkup_buttonEnd;
            this.radioButtonNo.Text = Resources.ValidateMarkup_radioButtonNo;
            this.radioButtonYes.Text = Resources.ValidateMarkup_radioButtonYes;
        }

        private void ValidateMarkup_SizeChanged(object sender, EventArgs e) {
            this.richTextBoxOriginal.Width = this.Size.Width - 30;
            this.richTextBoxMarkup.Width = this.Size.Width - 30;
        }

        private void currencyManager_PositionChanged(object sender, EventArgs e) {
            /* Verificamos si la etiquetas de la cita estan coloreadas y si no loas coloreamos */
            if (!this.citas[this.currencyManager.Position].Colorized) {
                this.colorRefTagsForm(this.structNode, startColor);
                this.citas[this.currencyManager.Position].Colorized = true;
            }
            /* Llamada showNavButtons */
            this.showNavButtons();
            /* Mostramos la posicion del resultado actual respecto al total */
            this.citationOf.Text = String.Format(Resources.ValidateMarkup_citationOf, (this.currencyManager.Position + 1), this.citas.Count);
        }

        private void buttonFirst_Click(object sender, EventArgs e)
        {
            if (this.currencyManager.Position != 0)
            {
                this.currencyManager.Position = 0;
            }
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (this.currencyManager.Position != 0) {
                this.currencyManager.Position--;
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (this.currencyManager.Position != (this.citas.Count - 1)) {
                this.currencyManager.Position++;
            }
        }

        private void buttonLast_Click(object sender, EventArgs e)
        {
            if (this.currencyManager.Position != (this.citas.Count - 1))
            {
                this.currencyManager.Position = this.citas.Count - 1;
            }
        }

        #region showNavButtons
        /// <summary>
        /// Habilitamos o desabilitamos los botones Primera, Anterior, Siguiente, Última dependiendo el numero de citas
        /// </summary>
        private void showNavButtons() {
            if (this.citas.Count == 1) { 
                this.buttonFirst.Enabled = false;
                this.buttonPrev.Enabled = false;
                this.buttonNext.Enabled = false;
                this.buttonLast.Enabled = false;
            }
            else if (this.citas.Count > 1 && this.currencyManager.Position == 0) {
                this.buttonFirst.Enabled = false;
                this.buttonPrev.Enabled = false;
                this.buttonNext.Enabled = true;
                this.buttonLast.Enabled = true;
            }
            else if (this.citas.Count > 1 && this.currencyManager.Position == this.citas.Count - 1)
            {
                this.buttonFirst.Enabled = true;
                this.buttonPrev.Enabled = true;
                this.buttonNext.Enabled = false;
                this.buttonLast.Enabled = false;
            }
            else {
                this.buttonFirst.Enabled = true;
                this.buttonPrev.Enabled = true;
                this.buttonNext.Enabled = true;
                this.buttonLast.Enabled = true;
            }
        }
        #endregion

        #region colorTagsInForm
        /// <summary>
        /// Function to colorize tags in form
        /// </summary>
        /// <param name="structNode">Contenido de la etiqueta actual con sus hijos</param>
        /// <param name="color">Color de la etiqueta {0,...,4} el -1 sirver para iniciar </param>
        public void colorRefTagsForm(XmlNode structNode, int color)
        {
            Regex tagExp = null;
            RegexOptions options = RegexOptions.IgnoreCase;
            Match matchResults = null;
            String startTag = null;
            /* Definimos y asignamos el arreglo de colores para las etiquetas */
            Color[] colors = new Color[]{
                Color.DarkBlue,
                Color.Teal,
                Color.Gray,
                Color.Blue,
                Color.Violet
            };
            /* Si el color inicial es -1 formateamos el texto con la fuente estandar y asignamos color=0 */
            if(color == -1){
                richTextBoxMarkup.SelectAll();
                richTextBoxMarkup.SelectionFont = new Font("Verdana", 10, FontStyle.Regular);
                richTextBoxMarkup.SelectionColor = Color.Black;
                color = 0;
            }
           
            /* Iteramos las etiquetas(tags) hijas*/
            /* Si la estructura enviada contiene una etiqueta aumentamos el numero de color para las etiquetas hijas */
            if (structNode.Attributes.GetNamedItem("tag") != null)
            {
                color++;
                /* Si el indice de color llego a 20 lo reiniciamos a 0 */
                color = color == 5 ? 0 : color;
            }
            foreach (XmlNode tag in structNode.ChildNodes)
            {
                try
                {
                    /* Al igual que en marcado analizamos la condiciones existentes en donde se tienen que marcar etiquetas(tags) hijas */
                    if (tag.SelectSingleNode("value") == null && tag.ChildNodes.Count > 0)
                    {
                        this.colorRefTagsForm(tag, color);
                    }
                    else
                    {
                        if (tag.SelectSingleNode("multiple") != null && tag.ChildNodes.Count > 0)
                        {
                            this.colorRefTagsForm(tag, color);
                        }
                        else if (tag.SelectSingleNode("regex") != null && tag.ChildNodes.Count > 0)
                        {
                            this.colorRefTagsForm(tag, color);
                        }
                    }
                    /* Si el atributo indica que es una etiqueta(tag) la coloreamos */
                    if (tag.Attributes != null && tag.Attributes.GetNamedItem("tag") != null)
                    {
                        tagExp = new Regex("\\[/*" + tag.Name + ".*?\\]", options);
                        matchResults = tagExp.Match(this.richTextBoxMarkup.Text);
                        while (matchResults.Success)
                        {
                            startTag = matchResults.Value;
                            
                            /* Buscamos y coloreamos el inicio o final de la etiqueta(tag) */
                            this.richTextBoxMarkup.Select(matchResults.Index, matchResults.Length);
                            richTextBoxMarkup.SelectionFont = new Font("Arial", 11, FontStyle.Regular);
                            richTextBoxMarkup.SelectionColor = colors[color];
                            matchResults = matchResults.NextMatch();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        #endregion

        #region Disable close button
        /// <summary>
        /// Sección de código para quitar el boron "x"
        /// </summary>

        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);

        private void ValidateMarkup_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);

            int menuItemCount = GetMenuItemCount(hMenu);
            /* Quitando boton cerrar "x" */
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
        }
        #endregion

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
