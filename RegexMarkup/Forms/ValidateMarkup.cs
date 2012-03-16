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
using RegexMarkup.Forms;


namespace RegexMarkup
{
    public partial class ValidateMarkup : Form
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static ValidateMarkup instance = null;
        static readonly object padlock = new object();

        public static ValidateMarkup Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ValidateMarkup();
                    }
                    return instance;
                }
            }
        }
        #endregion

        const int MF_BYPOSITION = 0x400;
        private List<MarkupStruct> citas;
        private CurrencyManager currencyManager = null;
        private int startColor = -1;
        private Dictionary<String, GroupBox> groupMarkupButtons = new Dictionary<String, GroupBox>();
        private DTDSciELO dtdSciELO = DTDSciELO.Instance;
        private SgmlDtd dtd = null;
        private Tags tags = Tags.Instance;
        private String citationStyle = "other";
        private String dtdVersion = null;
        private String dtdType = null;
        private String currentGroupTag = null;
        private EditAttribute editAttribute = null;
        private RichTextBox richTextBoxTemp = new RichTextBox();
        private int indexColorTag = 0;

        /* Definimos y asignamos el arreglo de colores para las etiquetas */
        private Color[] colors = new Color[]{
                Color.DarkBlue,
                Color.Teal,
                Color.Gray,
                Color.Blue,
                Color.Violet};

        public ValidateMarkup()
        {
            InitializeComponent();
            /* Agregando evento para cambiar el tamaño de los richtextbox cuando cambie el tamaño del formulario */
            this.SizeChanged += new EventHandler(ValidateMarkup_SizeChanged);
            /* Textos del formulario */
            this.Text = Resources.ValidateMarkup_title;
            this.groupBoxOriginal.Text = Resources.ValidateMarkup_groupBoxOriginal;
            this.groupBoxMarkup.Text = Resources.ValidateMarkup_groupBoxMarkup;
            this.radioButtonNo.Text = Resources.ValidateMarkup_radioButtonNo;
            this.radioButtonYes.Text = Resources.ValidateMarkup_radioButtonYes;
            this.labelCitationStatus.Text = Resources.ValidateMarkup_labelCitationStatus;
            this.groupBoxTools.Text = Resources.ValidateMarkup_groupBoxTools;
            /*Tooltip para los botones*/
            this.toolTipInfo.SetToolTip(this.buttonEnd, Resources.ValidateMarkup_buttonEndToolTip);
            this.toolTipInfo.SetToolTip(this.buttonCancel, Resources.ValidateMarkup_buttonCancelToolTip);
            this.toolTipInfo.SetToolTip(this.buttonClearTag, Resources.ValidateMarkup_buttonClearTagToolTip);
            this.toolTipInfo.SetToolTip(this.buttonEditAttr, Resources.ValidateMarkup_buttonEditAttrToolTip);
            this.toolTipInfo.SetToolTip(this.buttonUndo, Resources.ValidateMarkup_buttonUndo);
            this.toolTipInfo.SetToolTip(this.buttonRedo, Resources.ValidateMarkup_buttonRedo);
            this.toolTipInfo.SetToolTip(this.buttonFirst, Resources.ValidateMarkup_buttonFirstToolTip);
            this.toolTipInfo.SetToolTip(this.buttonPrev, Resources.ValidateMarkup_buttonPrevToolTip);
            this.toolTipInfo.SetToolTip(this.buttonNext, Resources.ValidateMarkup_buttonNextToolTip);
            this.toolTipInfo.SetToolTip(this.buttonLast, Resources.ValidateMarkup_buttonLastToolTip);
            /*Configuracion de los controles*/
            this.richTextBoxOriginal.DetectUrls = false; 
            this.richTextBoxMarkup.DetectUrls = false;
            this.richTextBoxMarkup.HideSelection = false;
            this.richTextBoxTemp.DetectUrls = false;
        }

        public void startValidate(ref List<MarkupStruct> citas)
        {
            this.citas = citas;
            /*Cargamos los datos de las citas*/
            this.currencyManager = (CurrencyManager)this.BindingContext[this.citas];
            this.richTextBoxOriginal.DataBindings.Clear();
            this.richTextBoxMarkup.DataBindings.Clear();
            this.radioButtonYes.DataBindings.Clear();
            this.radioButtonNo.DataBindings.Clear();
            this.richTextBoxOriginal.DataBindings.Add("Text", this.citas, "OriginalStr");
            this.richTextBoxMarkup.DataBindings.Add("Rtf", this.citas, "MarkedStrRtf");
            this.radioButtonYes.DataBindings.Add("Checked", this.citas, "Marked", false, DataSourceUpdateMode.OnPropertyChanged);
            this.radioButtonNo.DataBindings.Add("Checked", this.citas, "MarkedNo", false, DataSourceUpdateMode.OnPropertyChanged);
            /* Evento para colorear en la primera llamada */
            this.richTextBoxMarkup.BindingContextChanged += new EventHandler(this.currencyManager_PositionChanged);
            /* Evento para colorear cuando cambie la posicion */
            this.currencyManager.PositionChanged += new EventHandler(this.currencyManager_PositionChanged);
            /*Actualizamos la información de la DTD*/
            this.dtd = dtdSciELO.getDTD(this.dtdVersion, this.dtdType);
            this.tags.Dtd = this.dtd;
            /*Creando barra de herramientas para las etiquetas*/
            this.addMarkupButtons(this.citationStyle, null);
        }

        #region addMarkupButtons
        /// <summary>
        /// Coloca los botones de forma dinamica en el formulario a partir de un nodo padre
        /// </summary>
        public void addMarkupButtons(String node, String parentGroup) {
            ElementDecl article = null;
            Dictionary<String, MarkupButton> childs = null;
            node = node.ToLower();
            if (this.currentGroupTag != null)
            {
                this.groupMarkupButtons[this.currentGroupTag].Visible = false;
            }
            /*Asignamos el valor del grupo de etiquetas actual*/
            this.currentGroupTag = node;
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
                this.groupMarkupButtons[node].Location = new Point(10, 49);
                this.Controls.Add(this.groupMarkupButtons[node]);
                /*Posicion de los botones dentro del grupo*/
                int botonesPosX = 10;
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
                    this.toolTipInfo.SetToolTip(childs["parentGroup"].Markup, String.Format(Resources.ValidateMarkup_parentTag, parentGroup));
                }
                /*Llenamos un diccionario con los nodos hijos*/
                article = this.dtd.FindElement(node);
                foreach(String childName in this.tags.getChilds(node)){
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
                        if (this.tags.Tag[childName].ChildNodes)
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
                            this.toolTipInfo.SetToolTip(childs[childName].Childs, String.Format(Resources.ValidateMarkup_childTags, childName));
                        }
                    }
                }
                /* Agregamos los botones al grupo*/
                foreach(String childName in childs.Keys){
                    /*Agregamos el boton de la etiqueta*/
                    childs[childName].Markup.Location = new Point(botonesPosX, 15);
                    this.groupMarkupButtons[node].Controls.Add(childs[childName].Markup);
                    botonesPosX += childs[childName].Markup.Size.Width;
                    /*Si la etiqueta tiene nodos hijos y el nodo padre no es el mismo que la tiqueta agregamos el botón para mostrar los nodos hijos*/
                    if (childs[childName].Childs != null && node != childs[childName].Markup.Name)
                    {
                        childs[childName].Childs.Location = new Point(botonesPosX, 15);
                        this.groupMarkupButtons[node].Controls.Add(childs[childName].Childs);
                        botonesPosX += childs[childName].Childs.Size.Width;
                    }
                }
            }
            else {
                this.groupMarkupButtons[node].Visible = true;
                /*Si el nodo padre no es nulo y ademas es diferente del actual lo actualizamos*/
                if (parentGroup != null && parentGroup != this.groupMarkupButtons[node].Controls[0].Name) {
                    this.groupMarkupButtons[node].Controls[0].Name = parentGroup + "ParentGrp";
                    this.toolTipInfo.SetToolTip(this.groupMarkupButtons[node].Controls[0], String.Format(Resources.ValidateMarkup_parentTag, parentGroup));
                }
            }

        }

        #endregion

        #region clearTag
        /// <summary>
        /// Funcion que limpia las etiquetas dentro de una cadena
        /// </summary>
        /// <param name="originalString">La cadena a la que se le quitaran la etiqueta</param>
        /// <param name="tag">La etiqueta que eliminaremos</param>
        /// <returns></returns>

        private String clearTag(String originalString, String node){
            Regex objRegExp = null;
            RegexOptions options = RegexOptions.None;
            Match matchResults = null;
            bool tagReplaced = false;
            objRegExp = new Regex(@"\[/?" + node + @"[^\]]*?\]", options);
            matchResults = objRegExp.Match(originalString);
            while (matchResults.Success) {
                originalString = originalString.Replace(matchResults.Value, "");
                tagReplaced = true;
                matchResults = matchResults.NextMatch();
            }
            if (tagReplaced && this.tags.Tag[node].ChildNodes) {
                foreach (String tagName in this.tags.Tag[node].Childs) {
                    originalString = this.clearTag(originalString, tagName);
                }
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
            int panelNavigationY = this.panelNavigation.Location.Y;
            int panelNavigationX = (this.Size.Width - this.panelNavigation.Size.Width) / 2;
            this.groupBoxOriginal.Width = this.Size.Width - 35;
            this.groupBoxMarkup.Width = this.Size.Width - 35;
            this.groupMarkupButtons[this.currentGroupTag].MaximumSize = new System.Drawing.Size(this.Size.Width - 34, 45);
            this.panelNavigation.Location = new System.Drawing.Point(panelNavigationX, panelNavigationY);
        }

        private void currencyManager_PositionChanged(object sender, EventArgs e) {
            /* Verificamos si la etiquetas de la cita estan coloreadas y si no loas coloreamos */
            if (!this.citas[this.currencyManager.Position].Colorized) {
                this.colorRefTagsForm(this.citationStyle, startColor);
                this.citas[this.currencyManager.Position].Colorized = true;
            }
            /* Llamada showNavButtons */
            this.showNavButtons();
            /* Mostramos la posicion del resultado actual respecto al total */
            this.citationOf.Text = String.Format(Resources.ValidateMarkup_citationOf, (this.currencyManager.Position + 1), this.citas.Count);
        }
        #endregion

        #region selectTagContent
        /// <summary>
        /// Selecciona el texto comprendido entre una etiqueta
        /// </summary>
        /// <param name="node">Etiqueta de la haremos la selección</param>
        private void selectTagContent(String node) {
            String endTag = null;
            int startOriginalSelection = 0;
            int startSelection = 0;
            int endSelection = 0;
            /*Buscando el inicio de la etiqueta seleccionada*/
            startOriginalSelection = this.richTextBoxMarkup.SelectionStart;
            this.richTextBoxMarkup.Select(startOriginalSelection - 1, 1);
            if (this.richTextBoxMarkup.SelectedText == "[")
            {
                endTag = "[/" + node + "]";
                startSelection = this.richTextBoxMarkup.SelectionStart;
                endSelection = this.richTextBoxMarkup.Find(endTag, startOriginalSelection, RichTextBoxFinds.None) + endTag.Length;
            }
            else
            {
                startSelection = this.richTextBoxMarkup.Find("[" + node, 0, startOriginalSelection, RichTextBoxFinds.Reverse);
                endSelection = this.richTextBoxMarkup.Find("]", startOriginalSelection, RichTextBoxFinds.None) + 1;
            }
            /*Seleccionando cadena que comprende a la etiqueta seleccionada*/
            this.richTextBoxMarkup.Select(startSelection, endSelection - startSelection);
        }
        #endregion

        #region updateIndexColorTag
        /// <summary>
        /// Nos permite actualizar el valor de indexColorTag sin pasarnos del margen 0-4
        /// </summary>
        /// <param name="increment">Incremento o decremento de indexColorTag</param>
        private void updateIndexColorTag(int increment) {

            this.indexColorTag += increment;
            if (this.indexColorTag > 4 || this.indexColorTag < 0) {
                this.indexColorTag = 0;
            }
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
        public void colorRefTagsForm(String node, int color)
        {
            Regex tagExp = null;
            RegexOptions options = RegexOptions.IgnoreCase;
            Match matchResults = null;
            String startTag = null;
            bool TagSuccess = false;
            /* Si el color inicial es -1 formateamos el texto con la fuente estandar y asignamos color=0 */
            if (color == -1)
            {
                this.richTextBoxMarkup.SelectAll();
                this.richTextBoxMarkup.SelectionFont = new Font("Verdana", 10, FontStyle.Regular);
                this.richTextBoxMarkup.SelectionColor = Color.Black;
                color = 0;
            }
            else
            {
                /* Iteramos las etiquetas(tags) hijas*/
                color++;
            }
            /* Si el indice de color llego a 20 lo reiniciamos a 0 */
            color = color == 5 ? 0 : color;
            foreach (String tag in this.tags.getChilds(node))
            {
                try
                {
                    tagExp = new Regex("\\[/*" + tag + ".*?\\]", options);
                    matchResults = tagExp.Match(this.richTextBoxMarkup.Text);
                    while (matchResults.Success)
                    {
                        startTag = matchResults.Value;
                        
                        /* Buscamos y coloreamos el inicio o final de la etiqueta(tag) */
                        this.richTextBoxMarkup.Select(matchResults.Index, matchResults.Length);
                        this.richTextBoxMarkup.SelectionFont = new Font("Arial", 11, FontStyle.Regular);
                        this.richTextBoxMarkup.SelectionColor = this.colors[color];
                        TagSuccess = true;
                        matchResults = matchResults.NextMatch();
                    }
                    /* Si la etiqueta fue coloreada con exito coloreamos y el nodo tiene etiquetas hijas */
                    if (TagSuccess && this.tags.Tag[tag].ChildNodes)
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

        #region Click events function
        /// <summary>
        /// Conjuntos de funciones para los eventos click de los botones
        /// </summary>

        /*Funcion para para el evento click del boton que muestra los nodos hijos de la etiqueta*/
        private void buttonGetChilds_Click(object sender, EventArgs e)
        {
            Button senderChildsButton = (Button)sender;
            /*Actualizamos el valor de indexColorTag*/
            this.updateIndexColorTag(1);
            /*Lamamos al grupo de etiquetas del nodo*/
            this.addMarkupButtons(senderChildsButton.Name.Replace("Childs", ""), this.groupMarkupButtons[this.currentGroupTag].Name);
        }
        /*Funcion para para el evento click del boton que muestra el grupo donde esta el nodo padre*/
        private void buttonParentGroup_Click(object sender, EventArgs e)
        {
            Button senderButton = (Button)sender;
            /*Actualizamos el valor de indexColorTag*/
            this.updateIndexColorTag(-1);
            /*Lamamos al grupo de etiquetas padre*/
            this.addMarkupButtons(senderButton.Name.Replace("ParentGrp", ""), null);
        }
        /*Funcion que marca el texto seleccionado con la etiqueta*/
        private void markupButtonTag_Click(object sender, EventArgs e)
        {
            Button senderMarkupButton = (Button)sender;
            String tagName = senderMarkupButton.Name;
            String openTag = "[" + senderMarkupButton.Name + "]";
            String closeTag = "[/" + senderMarkupButton.Name + "]";
            if (this.richTextBoxMarkup.SelectedText != "" && this.richTextBoxMarkup.SelectedText != null)
            {
                /*Creamos la apertura y cierre de etiqueta con color*/
                this.richTextBoxTemp.Clear();
                this.richTextBoxTemp.Font = new Font("Arial", 11, FontStyle.Regular);
                this.richTextBoxTemp.ForeColor = this.colors[indexColorTag];
                this.richTextBoxTemp.Text = openTag + closeTag;
                /*Insetamos la seleccion entre las etiquetas*/
                this.richTextBoxTemp.Select(openTag.Length, 0);
                this.richTextBoxTemp.SelectedRtf = this.richTextBoxMarkup.SelectedRtf;
                this.richTextBoxTemp.SelectAll();
                /*Si la etiqueta tiene atributos los editamos en caso contrario solo reemplazamos*/
                if (this.tags.getAttributes(tagName) != null)
                {
                    this.editAttribute = EditAttribute.Instance;
                    this.editAttribute.TagName = tagName;
                    this.editAttribute.SelectedRtb.Rtf = this.richTextBoxTemp.SelectedRtf;
                    this.editAttribute.startEditAttribute();
                    this.editAttribute.ShowDialog();
                    /*Reeplazamos*/
                    this.richTextBoxMarkup.SelectedRtf = this.editAttribute.SelectedRtb.SelectedRtf;
                }
                else
                {
                    this.richTextBoxMarkup.SelectedRtf = this.richTextBoxTemp.SelectedRtf;
                }
            }
        }
        /*Funcion encargada de eliminar la etiqueta seleccionada al dar click al buttonClearTag*/
        private void buttonClearTag_Click(object sender, EventArgs e)
        {
            String selectedTag = this.richTextBoxMarkup.SelectedText.Trim();
            /*Verificamos si el texto seleccionado no es nulo y ademas corresponde a un etiqueta*/
            if (selectedTag != null && this.dtd.FindElement(selectedTag) != null)
            {
                if (MessageBox.Show(String.Format(Resources.ValidateMarkup_deleteTagMessage, selectedTag), Resources.ValidateMarkup_deleteTagCaption + selectedTag, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    /*Seleccionamos el texto comprendido entre la etiqueta*/
                    this.selectTagContent(selectedTag);
                    /*Quitamos la o las etiquetas*/
                    this.richTextBoxTemp.Clear();
                    this.richTextBoxTemp.Font = new Font("Verdana", 10, FontStyle.Regular);
                    this.richTextBoxTemp.ForeColor = Color.Black;
                    this.richTextBoxTemp.Text = this.clearTag(this.richTextBoxMarkup.SelectedText, selectedTag);
                    this.richTextBoxTemp.SelectAll();
                    this.richTextBoxMarkup.SelectedRtf = this.richTextBoxTemp.SelectedRtf;
                }
            }
            else
            {
                MessageBox.Show(Resources.ValidateMarkup_messageNoTagSelection);
            }

        }

        private void buttonEditAttr_Click(object sender, EventArgs e)
        {
            String selectedTag = this.richTextBoxMarkup.SelectedText.Trim();
            if (selectedTag != null && this.dtd.FindElement(selectedTag) != null)
            {
                if (this.tags.getAttributes(selectedTag) != null)
                {
                    this.selectTagContent(selectedTag);
                    this.editAttribute = EditAttribute.Instance;
                    this.editAttribute.TagName = selectedTag;
                    this.editAttribute.SelectedRtb.Rtf = this.richTextBoxMarkup.SelectedRtf;
                    this.editAttribute.startEditAttribute();
                    /*Reeplazamos*/
                    if (this.editAttribute.ShowDialog() == DialogResult.OK)
                    {
                        this.richTextBoxMarkup.SelectedRtf = this.editAttribute.SelectedRtb.SelectedRtf;
                    }
                }
                else
                {
                    MessageBox.Show(Resources.ValidateMarkup_messageTagWithoutAttr);
                }
            }
            else
            {
                MessageBox.Show(Resources.ValidateMarkup_messageNoTagSelection);
            }
        }

        private void buttonUndo_Click(object sender, EventArgs e)
        {
            this.richTextBoxMarkup.Undo();
        }

        private void buttonRedo_Click(object sender, EventArgs e)
        {
            this.richTextBoxMarkup.Redo();
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
            if (this.currencyManager.Position != 0)
            {
                this.currencyManager.Position--;
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (this.currencyManager.Position != (this.citas.Count - 1))
            {
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
            String confirmCitations = null;
            confirmCitations = "\n{";
            int citaActual = 1;
            foreach (MarkupStruct cita in this.citas) {
                if(cita.Marked && cita.Colorized){
                    confirmCitations += citaActual + ", ";
                }
                citaActual++;
            }
            confirmCitations = confirmCitations.Remove(confirmCitations.Length - 2);
            confirmCitations += "}";
            if (MessageBox.Show( String.Format(Resources.ValidateMarkup_messageConfirmation, confirmCitations), Resources.ValidateMarkup_messageConfirmationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                /*Actualizamos la última posición para que se guarden los cambios*/
                this.currencyManager.Position = this.currencyManager.Position;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            foreach (MarkupStruct cita in this.citas)
            {
                cita.Marked = false;
            }
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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

        #region Getters an Setters

        public String CitationStyle
        {
            get { return citationStyle; }
            set { citationStyle = value; }
        }

        public String DtdVersion
        {
            get { return dtdVersion; }
            set { dtdVersion = value; }
        }

        public String DtdType
        {
            get { return dtdType; }
            set { dtdType = value; }
        }

        #endregion

    }
}
