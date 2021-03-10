namespace Todo.Application.Extensions
{
    public class AppJwtSettings
    {
        public string Secret { get; set; }
        public int Expires { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
