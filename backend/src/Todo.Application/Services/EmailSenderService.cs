using Todo.Application.Extensions;
using Todo.Application.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Todo.Application.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly AppEmailSettings _appEmailSettings;

        public EmailSenderService(IOptions<AppEmailSettings> appEmailSettings)
        {
            _appEmailSettings = appEmailSettings.Value;
        }

        public async Task<bool> SendForgotPasswordEmail(string userEmail, string userName, string forgotPasswordToken)
        {
            var uriBuilder = new UriBuilder("https://frontend-todo.azurewebsites.net/account/reset-password");

            var values = HttpUtility.ParseQueryString(string.Empty);
            values["token"] = forgotPasswordToken;
            values["username"] = userName;

            uriBuilder.Query = values.ToString();

            var emailBody = new StringBuilder()
                .Append("<h1>Reset your Todo password</h1><br>")
                .Append("<p>We heard that you forgot your Todo password. <b>Don't worry about that!<br><p></b>")
                .Append("<p>You can use the following button to reset your password:<p><br><br>")
                .Append($"<a class=button href={uriBuilder}>Reset your password</a>")
                .ToString();

            return await SendMail(userEmail, "[Todo] Please reset your password", emailBody, true);
        }

        public async Task<bool> SendForgotUserNameEmail(string userEmail, string userName)
        {
            var emailBody = new StringBuilder()
                .Append("<h1>Your Todo username.</h1><br>")
                .Append("<p>We heard that you forgot your Todo username. <b>Don't worry about that!</b><p><br>")
                .Append($"<p>Here is your username: <b>{userName}</b><p>")
                .ToString();

            return await SendMail(userEmail, "[Todo] Here is your username", emailBody, true);
        }

        public async Task<bool> SendResetedPasswordEmail(string userEmail, string userName)
        {
            var emailBody = new StringBuilder()
                .Append($"<p>Hello, {userName}")
                .Append($"<p>We wanted you to know that your Todo password was reseted.")
                .ToString();

            return await SendMail(userEmail, "[Todo] Your password was reseted", emailBody, true);
        }

        public async Task<bool> SendMail(string toEmailAddress, string emailSubject, string emailBody, bool isEmailBodyHtml)
        {
            var email = new MailMessage
            {
                From = new MailAddress(_appEmailSettings.Email)
            };

            email.To.Add(toEmailAddress);
            email.Subject = emailSubject;
            email.Body = emailBody;
            email.IsBodyHtml = isEmailBodyHtml;

            var smtp = new SmtpClient
            {
                UseDefaultCredentials = false,
                EnableSsl = _appEmailSettings.EnableSsl,
                Host = _appEmailSettings.Host,
                Port = _appEmailSettings.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_appEmailSettings.Email, _appEmailSettings.Password)
            };

            try
            {
                await smtp.SendMailAsync(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
