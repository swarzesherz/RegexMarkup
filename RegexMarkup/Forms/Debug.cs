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

namespace RegexMarkup.Forms
{
    public partial class Debug : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Debug()
        {
            InitializeComponent();
            this.SizeChanged += new EventHandler(Debug_SizeChanged);
            String debugFileDB = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.db");
            SQLiteConnection connection = new SQLiteConnection(@"Data Source=" + debugFileDB + "; Version=3; Compress=True;");
            connection.Open();

            String sql = "SELECT Date, Level, Class, Method, Message FROM Log ORDER BY Date DESC";

            SQLiteDataAdapter db = new SQLiteDataAdapter(sql, connection);

            DataSet ds = new DataSet();
            ds.Reset();

            DataTable dt = new DataTable();
            db.Fill(ds);

            dt = ds.Tables[0];
            this.dataGridViewLog.DataSource = dt;

            connection.Close();
        }

        void Debug_SizeChanged(object sender, EventArgs e)
        {
            this.dataGridViewLog.Size = new System.Drawing.Size(this.Size.Width - 10, this.Size.Height - 90);
        }

        private void Debug_Load(object sender, EventArgs e)
        {

        }


    }
}
