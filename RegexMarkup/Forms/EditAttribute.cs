﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RegexMarkup.Properties;

namespace RegexMarkup.Forms
{
    public sealed partial class EditAttribute : Form
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static EditAttribute instance = null;
        static readonly object padlock = new object();

        public static EditAttribute Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new EditAttribute();
                    }
                    return instance;
                }
            }
        }
        #endregion

        private RegexMarkupUtils utils = RegexMarkupUtils.Instance;
        private Tags tags = Tags.Instance;
        private String tagName = null;
        private RichTextBox selectedRtb = new RichTextBox();
        private Dictionary<String, GroupBox> groupAtributes = new Dictionary<String, GroupBox>();
        private Dictionary<String, CustomValidation.ContainerValidator> groupAttributesValidate = new Dictionary<String, CustomValidation.ContainerValidator>();
        private String actualAttibuteControl = null;
        private int startSelection = 0;
        private int lenghtSelection = 0;

        EditAttribute()
        {
            InitializeComponent();
            /* Icon */
            this.Icon = System.Drawing.Icon.FromHandle(global::RegexMarkup.Properties.Resources.pencil.GetHicon());
            /*Textos*/
            this.Text = Resources.EditAttribute_title;
            this.groupBoxTools.Text = Resources.groupBoxTools;
            /*Tooltip para los botones*/
            this.toolTipInfo.SetToolTip(this.buttonEdit, Resources.EditAttribute_buttonEditToolTip);
            this.toolTipInfo.SetToolTip(this.buttonCancel, Resources.EditAttribute_buttonCancelToolTip);
            /*Configuración de los controles*/
            this.selectedRtb.DetectUrls = false;
        }

        public void startEditAttribute(){
            Regex objRegExp = null;
            RegexOptions options = RegexOptions.None;
            Match matchResults = null;
            this.groupBoxTag.Text = this.tagName;
            /*Buscando el incio y find de cadena comprendidas entre las etiquetas*/
            objRegExp = new Regex(@"\[" + this.tagName + @"[^\]]*?\](.+?)\[/" + this.tagName + @"\]", options);
            matchResults = objRegExp.Match(this.selectedRtb.Text);
            while (matchResults.Success)
            {
                this.startSelection = matchResults.Groups[1].Index;
                this.lenghtSelection = matchResults.Groups[1].Length;
                matchResults = matchResults.NextMatch();
            }

            this.selectedRtb.Select(startSelection, lenghtSelection);
            this.richTextBoxTag.Rtf = this.selectedRtb.SelectedRtf;
            this.showAttributeControls();
            this.updateAttributeControls();
        }

        #region showAttributeControls
        /// <summary>
        /// Muestra los controles para cambiar los atributos de la etiqueta
        /// </summary>

        private void showAttributeControls() {
            String language = Resources.Culture.TwoLetterISOLanguageName;
            bool groupWithValidation = false;

            if (this.actualAttibuteControl != null) {
                this.groupAtributes[this.actualAttibuteControl].Visible = false;
            }
            this.actualAttibuteControl = this.tagName;
            /*Verificamos si el control no se ha creado anteriormente*/
            if (!this.groupAtributes.ContainsKey(this.tagName))
            {
                /*Agregamos el groupBox que contendra los atributos*/
                this.groupAtributes.Add(this.tagName, new GroupBox());
                this.groupAtributes[this.tagName].Name = this.tagName;
                this.groupAtributes[this.tagName].AutoSize = true;
                this.groupAtributes[this.tagName].AutoSizeMode = AutoSizeMode.GrowAndShrink;
                this.groupAtributes[this.tagName].MinimumSize = new System.Drawing.Size(this.Size.Width - 34, 45);
                this.groupAtributes[this.tagName].Location = new Point(10, 151);
                this.Controls.Add(this.groupAtributes[this.tagName]);
                int botonesPosY = 25;
                /*Buscamos y agregamos los atributos al groupBox*/
                foreach (KeyValuePair<String, AttrTagStruct> singleAttr in this.tags.Tag[this.tagName].Attributes)
                {
                    Label etiqueta = new Label();
                    TextBox textAttr = null;
                    ComboBox comboBoxAttr = null;

                    etiqueta.AutoSize = true;
                    etiqueta.Location = new System.Drawing.Point(10, botonesPosY);
                    etiqueta.TextAlign = ContentAlignment.MiddleLeft;
                    etiqueta.Name = "label" + singleAttr.Key;
                    etiqueta.Size = new System.Drawing.Size(63, 13);
                    etiqueta.Text = singleAttr.Key;
                    etiqueta.CausesValidation = false;
                    this.groupAtributes[this.tagName].Controls.Add(etiqueta);
                    /*Agregamos los controles necesarios deacuerdo a los valores de los atributos*/
                    if (this.tags.Tag[TagName].Attributes[singleAttr.Key].Values == null)
                    {
                        textAttr = new TextBox();
                        textAttr.Location = new System.Drawing.Point(70, botonesPosY);
                        textAttr.Size = new System.Drawing.Size(200, 13);
                        textAttr.TextAlign = HorizontalAlignment.Center;
                        textAttr.Name = "textBox" + singleAttr.Key;
                        this.groupAtributes[this.tagName].Controls.Add(textAttr);
                    }
                    else if (this.tags.Tag[TagName].Attributes[singleAttr.Key].Values[language].Count == 1)
                    {
                        textAttr = new TextBox();
                        textAttr.Location = new System.Drawing.Point(70, botonesPosY);
                        textAttr.Size = new System.Drawing.Size(200, 13);
                        textAttr.TextAlign = HorizontalAlignment.Center;
                        textAttr.Name = "textBox" + singleAttr.Key;
                        foreach (KeyValuePair<String, String> attrValue in this.tags.Tag[TagName].Attributes[singleAttr.Key].Values[language])
                        {
                            textAttr.Text = attrValue.Key;
                        }
                        this.groupAtributes[this.tagName].Controls.Add(textAttr);
                    }
                    else if (this.tags.Tag[TagName].Attributes[singleAttr.Key].Values[language].Count > 1)
                    {
                        BindingSource comboBoxAttrDS = null;
                        comboBoxAttrDS = new BindingSource(this.tags.Tag[TagName].Attributes[singleAttr.Key].Values[language], null);
                        comboBoxAttr = new ComboBox();
                        comboBoxAttr.Location = new System.Drawing.Point(70, botonesPosY);
                        comboBoxAttr.Name = "comboBox" + singleAttr.Key;
                        comboBoxAttr.BindingContext = new BindingContext();
                        comboBoxAttr.DisplayMember = "Value";
                        comboBoxAttr.ValueMember = "Key";
                        comboBoxAttr.DataSource = comboBoxAttrDS;
                        comboBoxAttr.Size = new System.Drawing.Size(this.dropDownWidth(comboBoxAttr), 13);
                        this.groupAtributes[this.tagName].Controls.Add(comboBoxAttr);
                    }
                    /*Si al atributo es requerido agregamos una validacion*/
                    if (singleAttr.Value.Presence == Sgml.AttributePresence.Required) {
                        groupWithValidation = true;
                        CustomValidation.RequiredFieldValidator requiredAttribute = new CustomValidation.RequiredFieldValidator();
                        ((System.ComponentModel.ISupportInitialize)(requiredAttribute)).BeginInit();
                        /*Agregamos los controles necesarios deacuerdo a los valores de los atributos*/
                        if (this.tags.Tag[TagName].Attributes[singleAttr.Key].Values == null) {
                            requiredAttribute.ControlToValidate = textAttr;
                        }
                        else if (this.tags.Tag[TagName].Attributes[singleAttr.Key].Values[language].Count == 1) {
                            requiredAttribute.ControlToValidate = textAttr;
                        }
                        else if (this.tags.Tag[TagName].Attributes[singleAttr.Key].Values[language].Count > 1) {
                            requiredAttribute.ControlToValidate = comboBoxAttr;
                        }
                        requiredAttribute.ErrorMessage = "Atributo Requerido";
                        requiredAttribute.Icon = Resources.requiredAttribute;
                        ((System.ComponentModel.ISupportInitialize)(requiredAttribute)).EndInit();
                    }
                    /*Incrementamos el la posicion en el eje Y*/
                    botonesPosY += etiqueta.Size.Height + 15;
                }
                /*Creamos la validación para el grupo si es requerida*/
                if (groupWithValidation)
                {
                    this.groupAttributesValidate.Add(this.tagName, new CustomValidation.ContainerValidator());
                    this.groupAttributesValidate[this.tagName].ContainerToValidate = this.groupAtributes[this.tagName];
                    this.groupAttributesValidate[this.tagName].HostingForm = this;
                    this.groupAttributesValidate[this.tagName].ValidationDepth = CustomValidation.ValidationDepth.ContainerOnly;
                }
            }
            else {
                this.groupAtributes[this.tagName].Visible = true;
            }

        }
        #endregion

        #region updateAttributeControls

        private void updateAttributeControls() {
            String attributeName = null;
            String attributeValue = null;
            foreach (Control attribute in this.groupAtributes[this.tagName].Controls){
                if (attribute.GetType().Name == "ComboBox")
                {
                    attributeName = ((ComboBox)attribute).Name.Replace("comboBox", "");
                    attributeValue = utils.getAttrValueInTag(this.tagName, attributeName, this.SelectedRtb.Text);
                    if (attributeValue != null && attributeValue != "")
                        ((ComboBox)attribute).SelectedValue = attributeValue;
                }
                else if (attribute.GetType().Name == "TextBox"){
                    attributeName = ((TextBox)attribute).Name.Replace("textBox", "");
                    attributeValue = utils.getAttrValueInTag(this.tagName, attributeName, this.SelectedRtb.Text);
                    if (attributeValue != null && attributeValue != "")
                        ((TextBox)attribute).Text = attributeValue;
                }
            }
        }
        #endregion

        #region dropDownWidth
        /// <summary>
        /// Funcion que nos devuelve el tamaño para ajustar un comboBox al texto
        /// </summary>
        /// <param name="myCombo">Recice el comboBox del cual queremos saber el tamaño</param>
        /// <returns></returns>
        private int dropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0;
            int temp = 0;
            Label label1 = new Label();

            foreach (KeyValuePair<String, String> item in myCombo.Items)
            {
                label1.Text = item.Value;
                temp = label1.PreferredWidth + 20;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            label1.Dispose();
            return maxWidth;
        }
        #endregion

        #region Getters and Setters

        public String TagName
        {
            get { return tagName; }
            set { tagName = value; }
        }

        public RichTextBox SelectedRtb
        {
            get { return selectedRtb; }
            set { selectedRtb = value; }
        }

        #endregion

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            String openTag = null;
            String attributeName = null;
            String attributeValue = null;
            openTag = "[" + this.tagName;
            bool groupWithoutValidation = false;
            /*Verficamos los atributos requeridos*/
            if (this.groupAttributesValidate.ContainsKey(this.actualAttibuteControl))
            {
                this.groupAttributesValidate[this.actualAttibuteControl].Validate();
            }
            else 
            {
                groupWithoutValidation = true;
            }

            if (groupWithoutValidation || this.groupAttributesValidate[this.actualAttibuteControl].IsValid)
            {
                /*Agregando atributos a la etiqueta de apertura*/
                foreach (Control attribute in this.groupAtributes[this.tagName].Controls)
                {
                    attributeName = null;
                    attributeValue = null;
                    if (attribute.GetType().Name == "ComboBox")
                    {
                        attributeName = ((ComboBox)attribute).Name.Replace("comboBox", "");
                        attributeValue = ((ComboBox)attribute).SelectedValue.ToString();
                        if (attributeValue != null && attributeValue != "")
                            openTag += " " + attributeName + "=\"" + attributeValue + "\"";
                    }
                    else if (attribute.GetType().Name == "TextBox")
                    {
                        attributeName = ((TextBox)attribute).Name.Replace("textBox", "");
                        attributeValue = ((TextBox)attribute).Text;
                        if (attributeValue != null && attributeValue != "")
                            openTag += " " + attributeName + "=\"" + attributeValue + "\"";
                    }
                }
                openTag += "]";
                /*Agregando la etiqueta con los atributos editados a selectedRtb*/
                this.selectedRtb.Select(0, this.startSelection);
                this.selectedRtb.SelectedText = openTag;
                this.selectedRtb.SelectAll();
                /*Cerramos este formulario*/
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            
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

        private void EditAttribute_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);

            int menuItemCount = GetMenuItemCount(hMenu);
            /* Quitando boton cerrar "x" */
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
        }
        #endregion

    }
}
