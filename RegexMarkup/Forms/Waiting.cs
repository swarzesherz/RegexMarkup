using System.Windows.Forms;
using RegexMarkup.Properties;
using System;
using System.Runtime.InteropServices;

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
            /* Icon */
            this.Icon = System.Drawing.Icon.FromHandle(global::RegexMarkup.Properties.Resources.hourglasshalf.GetHicon());
            /* Textos del formulario */
            this.Text = Resources.Waiting_labelWait;
            this.waitButton.Text = Resources.Waiting_labelWait;

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

        private void Waiting_Load(object sender, EventArgs e)
        {
            IntPtr hMenu = GetSystemMenu(this.Handle, false);

            int menuItemCount = GetMenuItemCount(hMenu);
            /* Quitando boton cerrar "x" */
            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
        }
        #endregion

    }
}
