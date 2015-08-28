using System;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Net.Mail;
using System.Windows.Forms;
using log4net;
using log4net.Appender;
using RegexMarkup.Properties;
using System.Text;
using LogViewer;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

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
        private String debugFileDB = null;
        private List<LogEntry> debugData = null;
        private int curretPage = 0;
        private int totalPages = 0;
        private int pageSize = 15;
        private int totalRows = 0;
        private MailMsg mailMsjForm = MailMsg.Instance;
        
        Debug()
        {
            InitializeComponent();
            /* Icon */
            this.Icon = System.Drawing.Icon.FromHandle(global::RegexMarkup.Properties.Resources.debug.GetHicon());

            this.SizeChanged += new EventHandler(Debug_SizeChanged);
            foreach (Object appender in LogManager.GetRepository().GetAppenders())
            {
                String appenderType = appender.GetType().Name;
                switch (appenderType)
                {
                    case "RollingFileAppender":
                        this.debugFileDB = ((RollingFileAppender)appender).File;
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
            LogEntryParser log = new LogEntryParser();
            FileStream stream = new FileStream(debugFileDB, FileMode.Open);
            IEnumerable<LogViewer.LogEntry> loge = log.Parse(stream);
            stream.Close();
            debugData = new List<LogViewer.LogEntry>(loge);
            debugData.Reverse();
            this.totalRows = debugData.Count;
            this.totalPages = (this.totalRows + this.pageSize - 1) / this.pageSize;
        }

        private void paginateLog(){
            int limit = this.pageSize * (this.curretPage + 1);
            this.pageOf.Text = String.Format(Resources.ValidateMarkup_citationOf, this.curretPage + 1, this.totalPages);
            this.showNavButtons();
            int start = this.curretPage * this.pageSize;
            DataTable dt = new DataTable();
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Level", typeof (String));
            dt.Columns.Add("Class", typeof (String));
            dt.Columns.Add("Method", typeof (String));
            dt.Columns.Add("Message", typeof (String));

            if (((this.curretPage + 1) * this.pageSize) > this.totalRows) {
                limit = this.totalRows;
            }

            for (int i = start; i < limit; i++)
            {
                LogEntry entry = debugData[i];
                dt.Rows.Add(entry.Data.TimeStamp, entry.Data.Level, entry.Data.LocationInfo.ClassName, entry.Data.LocationInfo.MethodName, entry.Data.Message);   
            }

            this.dataGridViewLog.DataSource = dt;
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
