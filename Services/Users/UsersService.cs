using AutoMapper;
using moneyManagerBE.Class;
using moneyManagerBE.Data;
using moneyManagerBE.Dtos;
using moneyManagerBE.Models;
using moneyManagerBE.Services.Hasher;

namespace moneyManagerBE.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IHasher _hasher;

        public UsersService(
            AppDbContext appDbContext,
            IMapper mapper,
            IHasher hasher
            )
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _hasher = hasher;
        }

        public DbResponse<UserDto> AddUser(User user)
        {
            bool userExist = CheckEmail(user.Email);

            if (userExist)
            {
                return new DbResponse<UserDto>
                {
                    IsSuccess = false,
                    Message = "This email address has already been registered"
                };
            }

            // hash the password
            string newlyHashedPassword = _hasher.HashPassword(user.Password);
            user.Password = newlyHashedPassword;

            _appDbContext.Users.Add(user);
            _appDbContext.SaveChanges();

            return new DbResponse<UserDto>
            {
                IsSuccess = true,
                Message = "Created user successful",
                Data = _mapper.Map<UserDto>(user)
            };
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

        public User? GetUserByEmail(string email)
        {
            return _appDbContext.Users.Where(theUser => theUser.Email == email).FirstOrDefault();
        }

        public bool CheckEmail(string emailAddress)
        {
            var foundUser = _appDbContext.Users.Where(theUser => theUser.Email == emailAddress).FirstOrDefault();

            if (foundUser != null)
            {
                return true;
            }

            return false;
        }
    }
}