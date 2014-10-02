using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.IO.Compression;
using System.Net.Mail;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using RegexMarkup.Properties;
using System.Text;

namespace RegexMarkup.Forms
{
    public sealed partial class Debug : Form
    {
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static Debug instance = null;
        static readonly object padlock = new object();

        public static Debug Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Debug();
                    }
                    return instance;
                }
            }
        }
        #endregion

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private String debugFileDB;
        private SQLiteConnection connection;
        private int curretPage;
        private int totalPages;
        private int queryLimit = 15;
        private MailMsg mailMsjForm = MailMsg.Instance;
        
        Debug()
        {
            InitializeComponent();
            this.SizeChanged += new EventHandler(Debug_SizeChanged);
            foreach (Object appender in LogManager.GetRepository().GetAppenders())
            {
                String appenderType = appender.GetType().Name;
                switch (appenderType)
                {
                    case "AdoNetAppender":
                        connection = new SQLiteConnection(((AdoNetAppender)appender).ConnectionString);
                        this.debugFileDB = ((AdoNetAppender)appender).ConnectionString;
                        this.debugFileDB = this.debugFileDB.Substring(this.debugFileDB.IndexOf("Data Source="));
                        this.debugFileDB = this.debugFileDB.Substring(this.debugFileDB.IndexOf('=') + 1);
                        this.debugFileDB = this.debugFileDB.Substring(0, this.debugFileDB.IndexOf(';')).Trim();
                        break;
                }
            }
            /*Actualizando cadenas de texto*/
            this.Text = Resources.Debug_title;
            this.groupBoxTools.Text = Resources.groupBoxTools;
            this.toolTipInfo.SetToolTip(this.buttonExit, Resources.Debug_buttonExitToolTip);
            this.toolTipInfo.SetToolTip(this.buttonSendMail, Resources.Debug_buttonSendMailToolTip);
        }

        private void constructPaginator(){
            this.connection.Open();
            String query = "SELECT count(*) FROM Log";
            SQLiteCommand cmd = new SQLiteCommand(query, this.connection);
            SQLiteDataReader datos = cmd.ExecuteReader();
            datos.Read();
            int registros = Convert.ToInt16(datos[0]);
            try
            {
                this.totalPages = (registros  + this.queryLimit - 1) / this.queryLimit;
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(ex.Message + "\r\n" + ex.StackTrace);
            }
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

        private void Debug_Load(object sender, EventArgs e)
        {
            this.constructPaginator();
            this.paginateLog();
        }

        private void sendDebugMail() {
            this.mailMsjForm.clear();
            if (this.mailMsjForm.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    Mail Cr = new Mail();
                    using (MailMessage message = new MailMessage())
                    {
                        message.Subject = "Regexmarkup.Debug";
                        message.To.Add(new MailAddress("achwazer@gmail.com"));
                        message.From = new MailAddress(Settings.Default.userEmail, "RegexMarkup SciELO - " + Settings.Default.userName + " (" + Settings.Default.userEmail + ")");
                        /* Comprimiendo y adjuntado archivo de la bítacora */
                        this.compressFile(this.debugFileDB, this.debugFileDB + ".gz");
                        /*Adjuntando archivo con compresion*/
                        message.Attachments.Add(new Attachment(this.debugFileDB + ".gz"));
                        message.Body = "   " + this.mailMsjForm.mailMsg;
                        message.Body += "\n   Mensaje con la bítacora \n\n   *VER EL ARCHIVO ADJUNTO*";
                        message.BodyEncoding = System.Text.Encoding.UTF8;
                        /* Enviar */
                        Cr.send(message);
                    }


                    MessageBox.Show("El Mail se ha Enviado Correctamente", "Listo!!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                catch (Exception ex)
                {
                    if (log.IsErrorEnabled) log.Error(ex.Message + "\n" + ex.StackTrace);
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void compressFile(String sourceFile, String destFile) {

            if (File.Exists(destFile)) {
                FileInfo fi = new FileInfo(destFile);
                File.Delete(destFile); 
            }
            /*Creando archivo temporal de la bítacora*/
            String tempFile = sourceFile + ".tmp";
            File.Copy(this.debugFileDB, tempFile, true);
            /*Comprimiendo el archivo temporal*/
            byte[] b;
            using (FileStream f = new FileStream(tempFile, FileMode.Open))
            {
                b = new byte[f.Length];
                f.Read(b, 0, (int)f.Length);
            }

            // C.
            // Use GZipStream to write compressed bytes to target file.
            using (FileStream f2 = new FileStream(destFile, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (GZipStream gz = new GZipStream(f2, CompressionMode.Compress, false))
            {
                gz.Write(b, 0, b.Length);
            }

            if (File.Exists(tempFile)) File.Delete(tempFile);
        }

        private void buttonSendMail_Click(object sender, EventArgs e)
        {
            this.sendDebugMail();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
