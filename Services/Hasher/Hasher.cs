namespace moneyManagerBE.Services.Hasher
{
    public class Hasher : IHasher
    {
        public string HashPassword(string password)
        {
            string hashedPassword = BC.EnhancedHashPassword(password, 13);

            return hashedPassword;
        }

        public bool VerifyHashPassword(string actualPassword, string hashedPassword)
        {
            return BC.EnhancedVerify(actualPassword, hashedPassword);
        }
    }
}