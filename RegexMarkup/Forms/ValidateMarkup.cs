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
        private List<MarkupStruct> citas;
        private CurrencyManager currencyManager = null;
        private XmlNode structNode = null;
        private int startColor = -1;
        private Dictionary<String, GroupBox> groupMarkupButtons = new Dictionary<String, GroupBox>();
        private DescriptionTags descriptionTags = DescriptionTags.Instance;
        private DTDSciELO dtdSciELO = DTDSciELO.Instance;
        private SgmlDtd dtd = null;
        private Tags tags = Tags.Instance;

        public ValidateMarkup(ref List<MarkupStruct> citas, ref XmlNode structNode)
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
            this.groupBoxOriginal.Text = Resources.ValidateMarkup_groupBoxOriginal;
            this.groupBoxMarkup.Text = Resources.ValidateMarkup_groupBoxMarkup;
            this.buttonFirst.Text = Resources.ValidateMarkup_buttonFirst;
            this.buttonLast.Text = Resources.ValidateMarkup_buttonLast;
            this.buttonPrev.Text = Resources.ValidateMarkup_buttonPrev;
            this.buttonNext.Text = Resources.ValidateMarkup_buttonNext;
            this.buttonEnd.Text = Resources.ValidateMarkup_buttonEnd;
            this.radioButtonNo.Text = Resources.ValidateMarkup_radioButtonNo;
            this.radioButtonYes.Text = Resources.ValidateMarkup_radioButtonYes;
            this.labelCitationStatus.Text = Resources.ValidateMarkup_labelCitationStatus;
            this.groupBoxTools.Text = Resources.ValidateMarkup_groupBoxTools;
            /*Tooltip para los botones*/
            this.toolTipInfo.SetToolTip(this.buttonClearTag, Resources.ValidateMarkup_buttonClearTagToolTip);
            this.toolTipInfo.SetToolTip(this.buttonEditAttr, Resources.ValidateMarkup_buttonEditAttrToolTip);
            /**/
            this.dtd = dtdSciELO.getDTD("4.0", "article");
            this.tags.Dtd = this.dtd;
            /*Creando barra de herramientas para las etiquetas*/
            this.addMarkupButtons("other", null);
            List<String> childs = this.tags.getChilds("other");
        }

        #region addMarkupButtons
        /// <summary>
        /// Coloca los botones de forma dinamica en el formulario a partir de un nodo padre
        /// </summary>
        public void addMarkupButtons(String node, String parentGroup) {
            ElementDecl article = null;
            Dictionary<String, MarkupButton> childs = null;
            node = node.ToLower();
            /* Verificamos si existe el grupo de botoes y si no lo creamos y agregamos los botones correspondientes*/
            if (!this.groupMarkupButtons.ContainsKey(node))
            {
                /*Creamos el grupo que sera el almacen de los botones del nodo*/
                this.groupMarkupButtons.Add(node, new GroupBox());
                this.groupMarkupButtons[node].Name = node;
                this.groupMarkupButtons[node].Text = node;
                this.groupMarkupButtons[node].AutoSize = true;
                this.groupMarkupButtons[node].AutoSizeMode = AutoSizeMode.GrowAndShrink;
                this.groupMarkupButtons[node].MaximumSize = new System.Drawing.Size(this.Size.Width - 34, 45);
                this.groupMarkupButtons[node].Location = new Point(10, 187);
                this.Controls.Add(this.groupMarkupButtons[node]);
                /*Posicion de los botones dentro del grupo*/
                int botonesPosY = 10;
                /*Instanciamos el Dictionary<String, MarkupButton>*/
                childs = new Dictionary<String, MarkupButton>();
                /*Agregamos un boton para regresar al grupo al que pertenece el nodo padre*/
                if (parentGroup != null)
                {
                    childs.Add("parentGroup", new MarkupButton());
                    childs["parentGroup"].Markup = new Button();
                    childs["parentGroup"].Markup.Name = parentGroup + "ParentGrp";
                    childs["parentGroup"].Markup.Image = Resources.upArrow;
                    childs["parentGroup"].Markup.AutoSize = true;
                    childs["parentGroup"].Markup.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    childs["parentGroup"].Markup.AutoEllipsis = false;
                    childs["parentGroup"].Markup.Margin = new System.Windows.Forms.Padding(0);
                    childs["parentGroup"].Markup.Padding = new System.Windows.Forms.Padding(0);
                    childs["parentGroup"].Markup.FlatStyle = FlatStyle.Flat;
                    childs["parentGroup"].Markup.FlatAppearance.BorderSize = 0;
                    childs["parentGroup"].Markup.Click += new EventHandler(this.buttonParentGroup_Click);
                    this.toolTipInfo.SetToolTip(childs["parentGroup"].Markup, "Regresar al nivel superior " + parentGroup);
                }
                /*Lenamos un diccionario con los nodos hijos*/
                dtd = dtdSciELO.getDTD("4.0", "article");
                article = dtd.FindElement(node);
                this.tags.getChilds(node);
                foreach(String childName in this.tags.Tag[node].Childs){
                    if (!childs.ContainsKey(childName))
                    {
                        childs.Add(childName, new MarkupButton());
                        childs[childName].Markup = new Button();
                        childs[childName].Markup.AutoSize = true;
                        childs[childName].Markup.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        childs[childName].Markup.AutoEllipsis = false;
                        childs[childName].Markup.Margin = new System.Windows.Forms.Padding(0);
                        childs[childName].Markup.Padding = new System.Windows.Forms.Padding(0);
                        childs[childName].Markup.Name = childName;
                        childs[childName].Markup.Text = childName;
                        childs[childName].Markup.Tag = childName;
                        childs[childName].Markup.FlatStyle = FlatStyle.Flat;
                        childs[childName].Markup.FlatAppearance.BorderSize = 0;
                        childs[childName].Markup.Click += new EventHandler(this.markupButtonTag_Click);
                        this.toolTipInfo.SetToolTip(childs[childName].Markup, tags.getDescription(childName));
                        if (this.tags.Tag[node].ChildNodes)
                        {
                            childs[childName].Childs = new Button();
                            childs[childName].Childs.AutoSize = true;
                            childs[childName].Childs.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                            childs[childName].Childs.AutoEllipsis = false;
                            childs[childName].Childs.Margin = new System.Windows.Forms.Padding(0);
                            childs[childName].Childs.Padding = new System.Windows.Forms.Padding(0);
                            childs[childName].Childs.Name = childName + "Childs";
                            childs[childName].Childs.Image = Resources.downArrow;
                            childs[childName].Childs.FlatStyle = FlatStyle.Flat;
                            childs[childName].Childs.FlatAppearance.BorderSize = 0;
                            childs[childName].Childs.Click += new EventHandler(this.buttonGetChilds_Click);
                            this.toolTipInfo.SetToolTip(childs[childName].Childs, "Nodos dentro de la etiqueta " + childName);
                        }
                    }
                }
                /* Agregamos los botones al grupo*/
                foreach(String childName in childs.Keys){
                    /*Agregamos el boton de la etiqueta*/
                    childs[childName].Markup.Location = new Point(botonesPosY, 15);
                    this.groupMarkupButtons[node].Controls.Add(childs[childName].Markup);
                    botonesPosY += childs[childName].Markup.Size.Width;
                    /*Si la etiqueta tiene nodos hijos y el nodo padre no es el mismo que la tiqueta agregamos el botón para mostrar los nodos hijos*/
                    if (childs[childName].Childs != null && node != childs[childName].Markup.Name)
                    {
                        childs[childName].Childs.Location = new Point(botonesPosY, 15);
                        this.groupMarkupButtons[node].Controls.Add(childs[childName].Childs);
                        botonesPosY += childs[childName].Childs.Size.Width;
                    }
                }
            }
            else {
                this.groupMarkupButtons[node].Visible = true;
                /*Si el nodo padre no es nulo y ademas es diferente del actual lo actualizamos*/
                if (parentGroup != null && parentGroup != this.groupMarkupButtons[node].Controls[0].Name) {
                    this.groupMarkupButtons[node].Controls[0].Name = parentGroup + "ParentGrp";
                    this.toolTipInfo.SetToolTip(this.groupMarkupButtons[node].Controls[0], "Regresar al nivel superior " + parentGroup);
                }
            }

        }

        #endregion

        #region 
        /// <summary>
        /// Funcion que limpia las etiquetas dentro de una cadena
        /// </summary>
        /// <param name="originalString">La cadena a la que se le quitaran la etiqueta</param>
        /// <param name="tag">La etiqueta que eliminaremos</param>
        /// <returns></returns>

        private String clearTag(String originalString, String tag){
            Regex objRegExp = null;
            RegexOptions options = RegexOptions.None;
            Match matchResults = null;
            bool tagReplaced = false;
            objRegExp = new Regex(@"\[/?" + tag + @"[^\]]*?\]", options);
            matchResults = objRegExp.Match(originalString);
            while (matchResults.Success) {
                originalString = originalString.Replace(matchResults.Value, "");
                tagReplaced = true;
                matchResults = matchResults.NextMatch();
            }
            if (tagReplaced) { 
                
            }
            return originalString;
        }
        #endregion

        #region General event functions
        /// <summary>
        /// Funciones para diferentes tipos de eventos
        /// </summary>
        
        /*Funcion que ajusta el tamaño de las cajas de texto cuando el formulario cambia de tamaño*/
        private void ValidateMarkup_SizeChanged(object sender, EventArgs e) {
            this.groupBoxOriginal.Width = this.Size.Width - 35;
            this.groupBoxMarkup.Width = this.Size.Width - 35;
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
        #endregion

        #region Click events function
        /// <summary>
        /// Conjuntos de funciones para los eventos click de los botones
        /// </summary>

        /*Funcion para para el evento click del boton que muestra los nodos hijos de la etiqueta*/
        private void buttonGetChilds_Click(object sender, EventArgs e)
        {
            Button senderChildsButton = (Button)sender;
            senderChildsButton.Parent.Visible = false;
            this.addMarkupButtons(senderChildsButton.Name.Replace("Childs", ""), senderChildsButton.Parent.Name);
        }
        /*Funcion para para el evento click del boton que muestra el grupo donde esta el nodo padre*/
        private void buttonParentGroup_Click(object sender, EventArgs e)
        {
            Button senderButton = (Button)sender;
            senderButton.Parent.Visible = false;
            this.addMarkupButtons(senderButton.Name.Replace("ParentGrp", ""), null);
        }
        /*Funcion que marca el texto seleccionado con la etiqueta*/
        private void markupButtonTag_Click(object sender, EventArgs e)
        {
            Button senderMarkupButton = (Button)sender;
            String openTag = "[" + senderMarkupButton.Name + "]";
            String closeTag = "[/" + senderMarkupButton.Name + "]";
            if (this.richTextBoxMarkup.SelectedText != "" && this.richTextBoxMarkup.SelectedText != null)
            {
                this.richTextBoxMarkup.SelectedText = openTag + this.richTextBoxMarkup.SelectedText + closeTag;
                this.currencyManager.Position = this.currencyManager.Position;
            }
        }
        /*Funcion encargada de eliminar la etiqueta seleccionada al dar click al buttonClearTag*/
        private void buttonClearTag_Click(object sender, EventArgs e)
        {
            String selectedTag = this.richTextBoxMarkup.SelectedText.Trim();
            String endTag = null;
            int startOriginalSelection = 0;
            int startSelection = 0;
            int endSelection = 0;
            /*Verificamos si el texto seleccionado no es nulo y ademas corresponde a un etiqueta*/
            if (selectedTag != null && this.dtd.FindElement(selectedTag) != null)
            {
                startOriginalSelection = this.richTextBoxMarkup.SelectionStart;
                this.richTextBoxMarkup.Select(startOriginalSelection - 1, 1);
                if (this.richTextBoxMarkup.SelectedText == "[")
                {
                    endTag = "[/" + selectedTag + "]";
                    startSelection = this.richTextBoxMarkup.SelectionStart;
                    endSelection = this.richTextBoxMarkup.Find(endTag, startOriginalSelection, RichTextBoxFinds.None) + endTag.Length;
                }
                else {
                    startSelection = this.richTextBoxMarkup.Find("[" + selectedTag, 0, startOriginalSelection, RichTextBoxFinds.Reverse);
                    endSelection = this.richTextBoxMarkup.Find("]", startOriginalSelection, RichTextBoxFinds.None) + 1;
                }
                this.richTextBoxMarkup.Select(startSelection, endSelection - startSelection);
                MessageBox.Show(this.richTextBoxMarkup.SelectedText);
                 MessageBox.Show(this.clearTag(this.richTextBoxMarkup.SelectedText, selectedTag));
                /*Buscando el inicio de la etiqueta seleccionada*/
                /*Seleccionando cadena que comprende a la etiqueta seleccionada*/

            }
            else {
                MessageBox.Show("La seleccion no corresponde a una etiqueta");
            }
            
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
        private void buttonEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

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
                    /* Si la etiqueta fue coloreada con exito coloreamos las etiquetas hijas */
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

    }
}
