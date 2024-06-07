using moneyManagerBE.Class;
using moneyManagerBE.Dtos;
using moneyManagerBE.Models;

namespace moneyManagerBE.Services.Authorization
{
    public interface IAuthorization
    {
        DbResponse<UserDto> AddUser(User user);
        DbResponse<LoginResponseDto> Login(string email, string password);


        bool CheckEmail(string emailAddress);

        DbResponse<bool> ForgotPassword(string email);

        DbResponse<bool> ChangePassword(string email, string newPassword);

        string GenerateToken();
    }
}