namespace moneyManagerBE.Services.Hasher
{
    public interface IHasher
    {
        string HashPassword(string password);
        bool VerifyHashPassword(string actualPassword, string hashedPassword);
    }
}