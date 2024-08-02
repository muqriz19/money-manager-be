using moneyManagerBE.Class;
using moneyManagerBE.Dtos;
using moneyManagerBE.Models;

namespace moneyManagerBE.Services.Authorization
{
    public interface IAuthorization
    {
        DbResponse<LoginResponseDto> Login(string email, string password);

        DbResponse<string> ForgotPasswordOperation(string email);

        DbResponse<bool> ChangePassword(string email, ResetPassword resetPassword);

        DbResponse<bool> CheckResetPasswordHashIsCorrect(string userEmail, string temporaryHashPassword);

        string GenerateToken();
    }
}