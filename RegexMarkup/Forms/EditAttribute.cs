using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RegexMarkup.Properties;

namespace RegexMarkup.Forms
{
    public partial class EditAttribute : Form
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

        private Tags tags = Tags.Instance;
        private String tagName = null;
        private String selectedRtb = null;
        private Dictionary<String, GroupBox> groupAtributes = new Dictionary<String, GroupBox>();
        private String actualAttibuteControl = null;

        public EditAttribute()
        {
            InitializeComponent();
        }

        public void startEditAttribute(){
            this.groupBoxTag.Text = this.tagName;
            this.richTextBoxTag.Rtf = this.selectedRtb;
            this.showAttributeControls();
        }

        private void showAttributeControls() {
            String language = Resources.Culture.TwoLetterISOLanguageName;
            if (this.actualAttibuteControl != null) {
                this.groupAtributes[this.actualAttibuteControl].Visible = false;
            }
            this.actualAttibuteControl = this.tagName;
            /*Verificamos si el control no se ha creado anteriormente*/
            if (!this.groupAtributes.ContainsKey(this.tagName))
            {
                this.groupAtributes.Add(this.tagName, new GroupBox());
                this.groupAtributes[this.tagName].Name = this.tagName;
                this.groupAtributes[this.tagName].AutoSize = true;
                this.groupAtributes[this.tagName].AutoSizeMode = AutoSizeMode.GrowAndShrink;
                this.groupAtributes[this.tagName].MinimumSize = new System.Drawing.Size(100, 45);
                this.groupAtributes[this.tagName].Location = new Point(10, 104);
                this.Controls.Add(this.groupAtributes[this.tagName]);
                int botonesPosY = 25;
                foreach (KeyValuePair<String, AttrTagStruct> singleAttr in this.tags.Tag[this.tagName].Attributes)
                {
                    Label etiqueta = new Label();
                    etiqueta.AutoSize = true;
                    etiqueta.Location = new System.Drawing.Point(10, botonesPosY);
                    etiqueta.TextAlign = ContentAlignment.MiddleLeft;
                    etiqueta.Name = "label" + singleAttr.Key;
                    etiqueta.Size = new System.Drawing.Size(63, 13);
                    etiqueta.Text = singleAttr.Key;
                    this.groupAtributes[this.tagName].Controls.Add(etiqueta);
                    /*Agregamos los controles necesarios deacuerdo a los valores de los atributos*/
                    if (this.tags.Tag[TagName].Attributes[singleAttr.Key].Values == null)
                    {
                        TextBox textAttr = new TextBox();
                        textAttr.Location = new System.Drawing.Point(70, botonesPosY);
                        textAttr.Size = new System.Drawing.Size(63, 200);
                        textAttr.TextAlign = HorizontalAlignment.Center;
                        textAttr.Name = "textBox" + singleAttr.Key;
                        this.groupAtributes[this.tagName].Controls.Add(textAttr);
                    }
                    else if (this.tags.Tag[TagName].Attributes[singleAttr.Key].Values[language].Count == 1)
                    {
                        TextBox textAttr = new TextBox();
                        textAttr.Location = new System.Drawing.Point(70, botonesPosY);
                        textAttr.Size = new System.Drawing.Size(63, 200);
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
                        ComboBox comboBoxAttr = new ComboBox();
                        comboBoxAttr.Location = new System.Drawing.Point(70, botonesPosY);
                        comboBoxAttr.Name = "comboBox" + singleAttr.Key;
                        //comboBoxAttr.AutoSize = true;
                        comboBoxAttr.BindingContext = new BindingContext();
                        comboBoxAttr.DisplayMember = "Value";
                        comboBoxAttr.ValueMember = "Key";
                        comboBoxAttr.DataSource = comboBoxAttrDS;
                        this.groupAtributes[this.tagName].Controls.Add(comboBoxAttr);
                    }
                    /*Incrementamos la posicion en el eje Y*/
                    botonesPosY += etiqueta.Size.Height + 15;
                }
            }
            else {
                this.groupAtributes[this.tagName].Visible = true;
            }
        }

        private void EditAttribute_Load(object sender, EventArgs e)
        {

        }

        #region Getters and Setters

        public String TagName
        {
            get { return tagName; }
            set { tagName = value; }
        }

        public String SelectedRtb
        {
            get { return selectedRtb; }
            set { selectedRtb = value; }
        }

        #endregion

    }
}
