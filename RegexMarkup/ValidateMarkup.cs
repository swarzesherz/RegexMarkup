using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RegexMarkup
{
    public partial class ValidateMarkup : Form
    {
        private List<markupStruct> citas;
        private CurrencyManager currencyManager = null;
        public ValidateMarkup(ref List<markupStruct> citas)
        {
            this.citas = citas;
            InitializeComponent();
            this.currencyManager = (CurrencyManager)this.BindingContext[this.citas];
            this.richTextBox1.DataBindings.Add("Text", this.citas, "OriginalStr");
            this.richTextBox2.DataBindings.Add("Text", this.citas, "MarkedStr");
            this.radioButton1.DataBindings.Add("Checked", this.citas, "Marked", false, DataSourceUpdateMode.OnPropertyChanged);
            this.radioButton2.DataBindings.Add("Checked", this.citas, "MarkedNo", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.currencyManager.Position = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.currencyManager.Position--;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.currencyManager.Position++;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.currencyManager.Position = this.citas.Count - 1;
        }

        private void ValidateMarkup_Load(object sender, EventArgs e)
        {

        }

    }
}
