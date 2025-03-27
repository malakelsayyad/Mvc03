using System.Net;
using System.Net.Mail;

namespace Company.G02.PL.Helpers
{
    public class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            //Mail Server : Gmail
            //SMTP

            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("malakelsayyad@gmail.com", "dvqccjmtibmxswuv"); //Sender
                client.Send("malakelsayyad@gmail.com", email.To, email.Subject, email.Body);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
