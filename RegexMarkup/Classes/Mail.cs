using System.Net.Mail;

namespace RegexMarkup
{
    class Mail
    {
        private SmtpClient server = new SmtpClient("smtp.gmail.com", 587);
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
            server.Credentials = new System.Net.NetworkCredential("scielo.regexmarkup@gmail.com", "5ci3l0#.");
            server.EnableSsl = true;
        }

        public void send(MailMessage message)
        {
            this.message = message;
            server.Send(this.message);
        }
    }
}
