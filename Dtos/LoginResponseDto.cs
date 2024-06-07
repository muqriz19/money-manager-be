

namespace moneyManagerBE.Dtos
{
    public class LoginResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public int UserId { get; set; }

        public DateTimeOffset CreatedDate { get; set; } = new DateTimeOffset();
    }
}