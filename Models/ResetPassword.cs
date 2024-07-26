namespace moneyManagerBE.Models
{
    public class ResetPassword
    {
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;

        public string ResetPasswordTempHash { get; set; } = string.Empty;
    }
}