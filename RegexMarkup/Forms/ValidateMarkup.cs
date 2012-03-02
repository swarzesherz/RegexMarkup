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
using Sgml;


namespace RegexMarkup
{
    public partial class ValidateMarkup : Form
    {
        const int MF_BYPOSITION = 0x400;
        private List<markupStruct> citas;
        private CurrencyManager currencyManager = null;
        private XmlNode structNode = null;
        private int startColor = -1;
        private Dictionary<String, GroupBox> groupMarkupButtons = new Dictionary<String, GroupBox>();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValidateMarkup));
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
            this.addMarkupButtons("other", null);
        }
        /*Coloca los botones de forma dinamica en el formulario a partir de un nodo padre*/
        public void addMarkupButtons(String node, String parentNode) {
            node = node.ToLower();
            /* Verificamos si existe el grupo de botoes y si no lo creamos y agregamos los botones correspondientes*/
            if (!this.groupMarkupButtons.ContainsKey(node))
            {
                ElementDecl article = DTDStruct.DTDScielo.FindElement(node);
                List<MarkupButton> childs = new List<MarkupButton>();
                /*Agregamos un boton para ver los botones del nodo padre*/
                if (parentNode != null)
                {
                    childs.Add(new MarkupButton());
                    childs[0].Markup = new Button();
                    childs[0].Markup.AutoSize = true;
                    childs[0].Markup.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    childs[0].Markup.Name = parentNode + "Parent";
                    childs[0].Markup.Image = Resources.upArrow;
                    childs[0].Markup.Tag = "Ir al nivel superior de" + parentNode;
                    childs[0].Markup.FlatStyle = FlatStyle.Flat;
                    childs[0].Markup.FlatAppearance.BorderSize = 0;
                    childs[0].Markup.Click += new EventHandler(this.buttonReturnParent_Click);
                }
                getChilds(article.ContentModel.CurrentModel, ref childs);
                /*Creamos el grupo que sera el almacen de los botones del nodo*/
                this.groupMarkupButtons.Add(node, new GroupBox());
                this.groupMarkupButtons[node].AutoSize = true;
                this.groupMarkupButtons[node].AutoSizeMode = AutoSizeMode.GrowAndShrink;
                this.groupMarkupButtons[node].SizeChanged += new EventHandler(groupMarkupButton_SizeChanged);
                this.groupMarkupButtons[node].Name = node;
                this.groupMarkupButtons[node].Text = node;
                this.groupMarkupButtons[node].Location = new Point(10, 165);
                this.Controls.Add(this.groupMarkupButtons[node]);
                /*Posicion de los botones dentro del grupo*/
                int botonesPosY = 10;
                /* Agregamos los botones al grupo*/
                for (int i = 0; i < childs.Count; i++)
                {
                    childs[i].Markup.Location = new Point(botonesPosY, 15);
                    this.groupMarkupButtons[node].Controls.Add(childs[i].Markup);
                    botonesPosY += childs[i].Markup.Size.Width;
                    if (childs[i].Childs != null)
                    {
                        childs[i].Childs.Location = new Point(botonesPosY, 15);
                        this.groupMarkupButtons[node].Controls.Add(childs[i].Childs);
                        botonesPosY += childs[i].Childs.Size.Width + 5;
                    }
                    else
                    {
                        botonesPosY += 5;
                    }
                }
            }
            else {
                this.groupMarkupButtons[node].Visible = true;
            }
            
        }
        /*Llena una lista con los nodos hijos como botones así como la creacíon de un boton para acceder a los nodos hijos de alguna etiqueta si fuera posible*/
        private void getChilds(Sgml.Group model, ref List<MarkupButton> childs)
        {
            foreach (Object child in model.CurrentMembers)
            {
                if (child.GetType().Namespace + "." + child.GetType().Name == "Sgml.Group")
                {
                    this.getChilds((Sgml.Group)child, ref childs);
                }
                else
                {
                    childs.Add(new MarkupButton());
                    int currentChild = childs.Count - 1;
                    String childName = child.ToString().ToLower();
                    childs[currentChild].Markup = new Button();
                    childs[currentChild].Markup.AutoSize = true;
                    childs[currentChild].Markup.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    childs[currentChild].Markup.Name = childName;
                    childs[currentChild].Markup.Text = childName;
                    childs[currentChild].Markup.Tag = childName;
                    childs[currentChild].Markup.FlatStyle = FlatStyle.Flat;
                    childs[currentChild].Markup.FlatAppearance.BorderSize = 0;
                    if (DTDStruct.DTDScielo.FindElement(childName).ContentModel.CurrentModel.CurrentMembers.Count > 0) {
                        childs[currentChild].Childs = new Button();
                        childs[currentChild].Childs.AutoSize = true;
                        childs[currentChild].Childs.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        childs[currentChild].Childs.Name = childName + "Childs";
                        childs[currentChild].Childs.Image = Resources.downArrow;
                        childs[currentChild].Childs.Tag = "Nodos dentro de la etiqueta" + childName;
                        childs[currentChild].Childs.FlatStyle = FlatStyle.Flat;
                        childs[currentChild].Childs.FlatAppearance.BorderSize = 0;
                        childs[currentChild].Childs.Click += new EventHandler(this.buttonGetChilds_Click);
                    }
                }
            }
        }
        /*Funcion para para el evento click del boton que muestra los nodos hijos de la etiqueta*/
        private void buttonGetChilds_Click(object sender, EventArgs e) {
            Button senderButton = (Button)sender;
            senderButton.Parent.Visible = false;
            this.addMarkupButtons(senderButton.Name.Replace("Childs", ""), senderButton.Parent.Name);
        }
        /*Funcion para para el evento click del boton que muestra el nodo padre*/
        private void buttonReturnParent_Click(object sender, EventArgs e)
        {
            Button senderButton = (Button)sender;
            senderButton.Parent.Visible = false;
            this.addMarkupButtons(senderButton.Name.Replace("Parent", ""), senderButton.Parent.Name);
        }
        /*Funcion que ajusta la altura de un groupBox cuando cambia de tamaño*/
        private void groupMarkupButton_SizeChanged(object sender, EventArgs e)
        {
            GroupBox senderGroupBox = (GroupBox)sender;
            this.groupMarkupButtons[senderGroupBox.Name].Height = 45;
        }
        /*Funcion que ajusta el tamaño de las cajas de texto cuando el formulario cambia de tamaño*/
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
            bool TagSuccess = false;
            /* Si el color inicial es -1 formateamos el texto con la fuente estandar y asignamos color=0 */
            if(color == -1){
                richTextBoxMarkup.SelectAll();
                richTextBoxMarkup.SelectionFont = new Font("Verdana", 10, FontStyle.Regular);
                richTextBoxMarkup.SelectionColor = Color.Black;
                color = 0;
            }
           
            /* Iteramos las etiquetas(tags) hijas*/
            color++;
            /* Si el indice de color llego a 20 lo reiniciamos a 0 */
            color = color == 5 ? 0 : color;
            foreach (XmlNode tag in structNode.ChildNodes)
            {
                try
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
                        TagSuccess = true;
                        matchResults = matchResults.NextMatch();
                    }
                    /* Si la etiqueta fue coloreada con exito coloreamos la etiquetas hijas */
                    if (TagSuccess)
                    {
                        this.colorRefTagsForm(tag, color);
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
