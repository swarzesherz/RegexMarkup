using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace RegexMarkup
{
    public partial class ValidateMarkup : Form
    {
        private List<markupStruct> citas;
        private CurrencyManager currencyManager = null;
        private XmlNode structNode = null;
        private int startColor = -1;
        public ValidateMarkup(ref List<markupStruct> citas, ref XmlNode structNode)
        {
            this.citas = citas;
            this.structNode = structNode;
            InitializeComponent();
            this.currencyManager = (CurrencyManager)this.BindingContext[this.citas];
            this.richTextBox1.DataBindings.Add("Text", this.citas, "OriginalStr");
            this.richTextBox2.DataBindings.Add("Text", this.citas, "MarkedStr");
            this.radioButton1.DataBindings.Add("Checked", this.citas, "Marked", false, DataSourceUpdateMode.OnPropertyChanged);
            this.radioButton2.DataBindings.Add("Checked", this.citas, "MarkedNo", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.currencyManager.Position != 0)
            {
                this.currencyManager.Position = 0;
                this.colorRefTagsForm(this.structNode, startColor);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.currencyManager.Position != 0) {
                this.currencyManager.Position--;
                this.colorRefTagsForm(this.structNode, startColor);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.currencyManager.Position != (this.citas.Count - 1)) {
                this.currencyManager.Position++;
                this.colorRefTagsForm(this.structNode, startColor);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.currencyManager.Position != (this.citas.Count - 1))
            {
                this.currencyManager.Position = this.citas.Count - 1;
                this.colorRefTagsForm(this.structNode, startColor);
            }
        }

        #region colorTagsInForm
        /// <summary>
        ///  Function to colorize tags in form
        /// </summary>
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
                richTextBox2.SelectAll();
                richTextBox2.SelectionFont = new Font("Verdana", 10, FontStyle.Regular);
                richTextBox2.SelectionColor = Color.Black;
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
                    if (tag.Attributes.GetNamedItem("tag") != null)
                    {
                        tagExp = new Regex("\\[/*" + tag.Name + ".*?\\]", options);
                        matchResults = tagExp.Match(this.richTextBox2.Text);
                        while (matchResults.Success)
                        {
                            startTag = matchResults.Value;
                            
                            /* Buscamos y coloreamos el inicio o final de la etiqueta(tag) */
                            this.richTextBox2.Select(matchResults.Index, matchResults.Length);
                            richTextBox2.SelectionFont = new Font("Arial", 11, FontStyle.Regular);
                            richTextBox2.SelectionColor = colors[color];
                            matchResults = matchResults.NextMatch();
                        }
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message);
                }
            }
        }
        #endregion
    }
}
