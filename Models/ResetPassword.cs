namespace moneyManagerBE.Models
{
    public class ResetPassword
    {
        public string Email { get; set; } = string.Empty;
        public string newPassword { get; set; } = string.Empty;
    }
}