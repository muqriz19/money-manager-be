namespace moneyManagerBE.Services.PasswordHasher
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyHashPassword(string actualPassword, string hashPassword);
    }
}