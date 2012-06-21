using System.Net.Mail;
using System.Configuration;
using System.Collections.Specialized;
using System;

namespace RegexMarkup
{
    class Mail
    {
        private SmtpClient server = null;
        private MailMessage message = null;
        #region Singleton Implement
        /// <summary>
        /// Código para llamar a la clase como un singleton
        /// </summary>
        static Mail instance = null;
        static readonly object padlock = new object();
        public static Mail Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Mail();
                    }
                    return instance;
                }
            }
        }
        #endregion
        public Mail() {
            NameValueCollection settings = ConfigurationManager.GetSection("mailSettings") as NameValueCollection;
            server = new SmtpClient(settings["smtpHost"], Int16.Parse(settings["smtpPort"]));
            server.Credentials = new System.Net.NetworkCredential(settings["user"], settings["password"]);
            server.EnableSsl = Boolean.Parse(settings["enbaleSsl"]);
        }

        public void send(MailMessage message)
        {
            this.message = message;
            server.Send(this.message);
        }
    }
}
