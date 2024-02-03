using moneyManagerBE.Class;
using moneyManagerBE.Models;

namespace moneyManagerBE.Services.Users
{
    public interface IUsersService
    {
        DbResponse<User> CheckUser(int userId);
    }
}