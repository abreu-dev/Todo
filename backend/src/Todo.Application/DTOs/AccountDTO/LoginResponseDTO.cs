namespace Todo.Application.DTOs.AccountDTO
{
    public class LoginResponseDTO : DTO
    {
        public string AccessToken { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
