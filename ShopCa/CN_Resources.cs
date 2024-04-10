using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace ShopCa
{
    public class CN_Resources
    {
        public static string CreatePassword()
        {
            string password = Guid.NewGuid().ToString("N").Substring(0,6);
            return password;
        }
        // encriptation SHA256
        public static string ConvertSha256(string text)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(text));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));

            }
            return Sb.ToString();
        }
        public static bool SendMail(string email, string subject, string message)
        {
            bool result = false;

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("gevirel@ac.sce.ac.il");
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("gevirel@ac.sce.ac.il", "emqs nudb iwfb scdt"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };
                smtp.Send(mail);
                result = true;

            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        public static string ConvertBase64(string path, out bool conversion)
        {
            string textBase64 = string.Empty;
            conversion = true;
            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                textBase64 = Convert.ToBase64String(bytes);

            }
            catch
            {
                conversion = false;
            }
            return textBase64;
        }
    
    }
}
