using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Threading.Tasks;


namespace sabatex.AspNetCore.Identity.UI.Services;

// This class is used by the application to send email for account confirmation and password reset.
// For more details see https://go.microsoft.com/fwlink/?LinkID=532713
public class EmailSender : IEmailSender
{
    public IConfiguration Configuration { get; }

    public EmailSender(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public async Task<string> SendEmailAsync(string email, string subject, string message)
    {
        var mailServer = Configuration.GetSection("MailServer");
        if (mailServer == null) return "Section <MailServer> is not configured.";
        try
        {
            var pass = mailServer.GetValue<string>("Pass");
            var login = mailServer.GetValue<string>("MailAdress");
            var host = mailServer.GetValue<string>("SMTPHost");
            var port = mailServer.GetValue<string>("SMTPPort");

            var smtpClient = new SmtpClient()
            {
                Host = host, // set your SMTP server name here
                Port = int.Parse(port), // Port 
                EnableSsl = true,
                Credentials = new NetworkCredential(login, pass)
            };
            using (var mail = new MailMessage(login, email, subject, message))
            {
                mail.IsBodyHtml = true;
                await smtpClient.SendMailAsync(mail);
            }
            return string.Empty;

        }
        catch (Exception ex)
        {
            return ex.Message;
        }


    }
    public async Task<string> SendConfirmEmailAsync(string email, string callbackUrl)
    {
        return await SendEmailAsync(email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
    }


}

