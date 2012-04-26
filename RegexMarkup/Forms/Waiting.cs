using System.Windows.Forms;
using RegexMarkup.Properties;

namespace RegexMarkup.Forms
{
    public sealed partial class Waiting : Form
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static Waiting instance = null;
        static readonly object padlock = new object();

        public static Waiting Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Waiting();
                    }
                    return instance;
                }
            }
        }
        #endregion
        Waiting()
        {
            InitializeComponent();
            /* Textos del formulario */
            this.Text = Resources.Waiting_labelWait;
            this.labelWait.Text = Resources.Waiting_labelWait;
        }
    }
}
