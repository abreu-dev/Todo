namespace Todo.Application.DTOs.AccountDTO
{
    public class ResetUserPasswordDTO : DTO
    {
        public string Username { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}
