using moneyManagerBE.Class;
using moneyManagerBE.Dtos;
using moneyManagerBE.Models;

namespace moneyManagerBE.Services.Users
{
    public interface IUsersService
    {
        public DbResponse<UserDto> AddUser(User user);
        DbResponse<User> CheckUser(int userId);
        User? GetUserByEmail(string email);

        bool CheckEmail(string emailAddress);
    }
}