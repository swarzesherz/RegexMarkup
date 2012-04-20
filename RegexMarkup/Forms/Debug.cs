using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;
using log4net;
using RegexMarkup.Properties;

namespace RegexMarkup.Forms
{
    public partial class Debug : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SQLiteConnection connection;
        private int curretPage;
        private int totalPages;
        private int queryLimit = 15;
        public Debug()
        {
            InitializeComponent();
            this.SizeChanged += new EventHandler(Debug_SizeChanged);
            String debugFileDB = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.db");
            connection = new SQLiteConnection(@"Data Source=" + debugFileDB + "; Version=3; Compress=True;");
            this.constructPaginator();
            this.paginateLog();
        }

        private void constructPaginator(){
            this.connection.Open();
            String query = "SELECT count() FROM Log";
            SQLiteCommand cmd = new SQLiteCommand(query, this.connection);
            SQLiteDataReader datos = cmd.ExecuteReader();
            this.totalPages = (Convert.ToInt16(datos[0]) + this.queryLimit -1 )/ this.queryLimit;
            this.connection.Close();
        }

        private void paginateLog(){
            this.pageOf.Text = String.Format(Resources.ValidateMarkup_citationOf, this.curretPage + 1, this.totalPages);
            this.showNavButtons();
            int start = this.curretPage * this.queryLimit;
            this.connection.Open();

            String query = String.Format("SELECT Date, Level, Class, Method, Message FROM Log ORDER BY Date DESC LIMIT {0}, {1}", start, this.queryLimit);

            SQLiteDataAdapter db = new SQLiteDataAdapter(query, this.connection);

            DataSet ds = new DataSet();
            ds.Reset();

            DataTable dt = new DataTable();
            db.Fill(ds);

            dt = ds.Tables[0];
            this.dataGridViewLog.DataSource = dt;

            this.connection.Close();
        }

        private void Debug_SizeChanged(object sender, EventArgs e)
        {
            int panelNavigationY = this.Size.Height - 80;
            int panelNavigationX = (this.Size.Width - this.panelNavigation.Size.Width) / 2;
            this.panelNavigation.Location = new System.Drawing.Point(panelNavigationX, panelNavigationY);
            this.dataGridViewLog.Size = new System.Drawing.Size(this.Size.Width - 20, this.Size.Height - 130);
        }

        #region showNavButtons
        /// <summary>
        /// Habilitamos o desabilitamos los botones Primera, Anterior, Siguiente, Última dependiendo el numero de citas
        /// </summary>
        private void showNavButtons()
        {
            if (this.totalPages == 1)
            {
                this.buttonFirst.Enabled = false;
                this.buttonPrev.Enabled = false;
                this.buttonNext.Enabled = false;
                this.buttonLast.Enabled = false;
            }
            else if (this.totalPages > 1 && this.curretPage == 0)
            {
                this.buttonFirst.Enabled = false;
                this.buttonPrev.Enabled = false;
                this.buttonNext.Enabled = true;
                this.buttonLast.Enabled = true;
            }
            else if (this.totalPages > 1 && this.curretPage == this.totalPages - 1)
            {
                this.buttonFirst.Enabled = true;
                this.buttonPrev.Enabled = true;
                this.buttonNext.Enabled = false;
                this.buttonLast.Enabled = false;
            }
            else
            {
                this.buttonFirst.Enabled = true;
                this.buttonPrev.Enabled = true;
                this.buttonNext.Enabled = true;
                this.buttonLast.Enabled = true;
            }
        }
        #endregion

        private void buttonNext_Click(object sender, EventArgs e)
        {
            this.curretPage++;
            this.paginateLog();
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            this.curretPage--;
            this.paginateLog();
        }

        private void buttonLast_Click(object sender, EventArgs e)
        {
            this.curretPage = this.totalPages - 1;
            this.paginateLog();
        }

        private void buttonFirst_Click(object sender, EventArgs e)
        {
            this.curretPage = 0;
            this.paginateLog();
        }


    }
}
