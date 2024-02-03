using Microsoft.EntityFrameworkCore;
using moneyManagerBE.Class;
using moneyManagerBE.Data;
using moneyManagerBE.Models;

namespace moneyManagerBE.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext _appDbContext;
        public UsersService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public DbResponse<User> CheckUser(int userId)
        {
            if (userId == 0)
            {
                return new DbResponse<User>
                {
                    IsSuccess = false,
                    Message = "User 0 is not correct"
                };
            }

            var foundUser = _appDbContext.Users.Where(user => user.Id == userId).FirstOrDefault();

            if (foundUser == null)
            {
                return new DbResponse<User>
                {
                    IsSuccess = false,
                    Message = $"User {userId} does not exist"
                };
            }

            return new DbResponse<User>
            {
                IsSuccess = true,
                Message = $"User {userId} exists",
                Data = foundUser
            };
        }
    }
}