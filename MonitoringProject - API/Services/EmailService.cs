using Microsoft.Extensions.Configuration;
using MonitoringProject___API.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringProject___API.Services
{
    public class EmailService
    {
        private IConfiguration config;
        private readonly MyContext context;
        private static string Mail, Password, Host, Port;

        public EmailService(IConfiguration config, MyContext context)
        {
            this.config = config;
            
            Host = config.GetSection("EmailSettings").GetSection("Host").Value;
            Port = config.GetSection("EmailSettings").GetSection("Port").Value;
            Mail = config.GetSection("EmailSettings").GetSection("Mail").Value;
            Password = config.GetSection("EmailSettings").GetSection("Password").Value;

            this.context = context;
        }

        public void SendEmail(string From, string Subject, string To, string url)
        {
            //var parameter = context.Parameters.Find(1);

            MailMessage mail = new MailMessage();
            mail.To.Add(To);
            mail.From = new MailAddress(From);
            mail.Subject = Subject;

            var builder = new StringBuilder();
            using(var reader = File.OpenText(Directory.GetCurrentDirectory() + "\\Services\\ResetPasswordTemplate.html"))
            {
                builder.Append(reader.ReadToEnd());
            }
            builder.Replace("{{url}}", url);
            mail.Body = builder.ToString();
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = Host;
            smtp.Port = Convert.ToInt32(Port);
            smtp.Credentials = new NetworkCredential(Mail, Password);
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }


    }
}
