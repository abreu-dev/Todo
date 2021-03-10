using System.Threading.Tasks;

namespace Todo.Application.Interfaces
{
    public interface IEmailSenderService
    {
        Task<bool> SendForgotPasswordEmail(string userEmail, string userName, string forgotPasswordToken);
        Task<bool> SendForgotUserNameEmail(string userEmail, string userName);
        Task<bool> SendResetedPasswordEmail(string userEmail, string userName);
    }
}
