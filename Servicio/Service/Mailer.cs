using System.Configuration;
using System.Net.Mail;

namespace Service
{
    public abstract class Mailer
    {
        #region ATRIBUTOS

        private static string origen = ConfigurationManager.AppSettings.Get("mailFrom");
        private static string destino = ConfigurationManager.AppSettings.Get("mailTo");
        private static string copia = ConfigurationManager.AppSettings.Get("mailCC");
        private static string pass = ConfigurationManager.AppSettings.Get("mailPass");
        private static string subject = ConfigurationManager.AppSettings.Get("asunto");
        private static string body = ConfigurationManager.AppSettings.Get("cuerpo");

        #endregion

        #region PROPIEDADES

        public string Body
        {
            get { return body; }
        }

        public string Subject
        {
            get { return subject; }
        }

        public string Copia
        {
            get { return copia; }
        }

        public string Destino
        {
            get { return destino; }
        }

        public string Origen
        {
            get { return origen; }
        }

        #endregion

        #region MÉTODOS PÚBLICOS

        /// <summary>
        /// Genera y envía el mail con el subject y body deseados.
        /// Toma los valores de origen y destino del archivo de configuración.
        /// </summary>
        /// <param name="customSubject"></param>
        /// <param name="customBody"></param>
        public static void SendMail(string customSubject, string customBody)
        {
            if (!string.IsNullOrEmpty(destino))
            {
                MailMessage omail = new MailMessage();

                omail.From = new MailAddress(origen);
                omail.To.Add(destino);

                if (!string.IsNullOrEmpty(copia))
                    omail.CC.Add(copia);

                omail.Subject = subject + customSubject;
                omail.Body = body + customBody;

                SmtpClient smtpclie = new SmtpClient("smtp.gmail.com");
                smtpclie.EnableSsl = true;
                smtpclie.UseDefaultCredentials = false;
                smtpclie.Host = "smtp.gmail.com";
                smtpclie.Port = 587;
                smtpclie.Credentials = new System.Net.NetworkCredential(origen, pass);

                smtpclie.Send(omail);

                omail.Dispose();
                smtpclie.Dispose();
            }
        }

        #endregion
    }
}
