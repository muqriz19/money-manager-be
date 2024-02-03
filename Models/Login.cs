namespace moneyManagerBE.Models
{
    public class Login
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginData
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public int UserId { get; set; }

        public DateTimeOffset CreatedDate { get; set; } = new DateTimeOffset();
    }
}