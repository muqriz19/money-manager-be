using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using moneyManagerBE.Class;
using moneyManagerBE.Data;
using moneyManagerBE.Dtos;
using moneyManagerBE.Models;
using moneyManagerBE.Services.PasswordHasher;

namespace moneyManagerBE.Services.Authorization
{
    public class AuthorizationService : IAuthorization, IPasswordHasher
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        private readonly string _randomPasswordText = "RANDOM_RESET_PASSWORD";

        private readonly IConfiguration _config;

        public AuthorizationService(AppDbContext appDbContext, IMapper mapper, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _config = configuration;
        }

        public DbResponse<UserDto> AddUser(User user)
        {
            bool userExists = CheckEmail(user.Email);

            if (userExists)
            {
                return new DbResponse<UserDto>
                {
                    IsSuccess = false,
                    Message = "This email address has already been registered, try another email"
                };
            }
            else
            {
                // hash the password
                string newlyHashedPassword = HashPassword(user.Password);
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
        }

        public DbResponse<LoginData> Login(string username, string password)
        {
            var foundUser = _appDbContext.Users.Where(user => user.Name == username).FirstOrDefault();

            if (foundUser != null)
            {
                if (VerifyHashPassword(password, foundUser.Password))
                {
                    return new DbResponse<LoginData>
                    {
                        IsSuccess = true,
                        Message = "Valid login",
                        Data = new LoginData
                        {
                            Name = foundUser.Name,
                            Email = foundUser.Email,
                            CreatedDate = foundUser.CreatedDate
                        }
                    };
                }
                else
                {
                    return new DbResponse<LoginData>
                    {
                        IsSuccess = false,
                        Message = "Invalid login"
                    };
                }

            }
            else
            {
                return new DbResponse<LoginData>
                {
                    IsSuccess = false,
                    Message = "This login does not exist"
                };
            }
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

        public DbResponse<bool> ForgotPassword(string email)
        {
            var foundUser = _appDbContext.Users.Where(user => user.Email == email).FirstOrDefault()!;

            foundUser.Password = HashPassword(GenerateRandomResetPassword());

            _appDbContext.Users.Update(foundUser);
            _appDbContext.SaveChanges();

            return new DbResponse<bool>
            {
                IsSuccess = true,
                Message = "Password resetted"
            };
        }

        public DbResponse<bool> ChangePassword(string email, string newPassword)
        {
            // reset the password - basically null for the first implementation - later improve
            var foundUser = _appDbContext.Users.Where(user => user.Email == email).FirstOrDefault()!;

            foundUser.Password = HashPassword(newPassword);

            _appDbContext.Users.Update(foundUser);
            _appDbContext.SaveChanges();

            return new DbResponse<bool>
            {
                IsSuccess = true,
                Message = "Password successfully changed"
            };
        }

        public string HashPassword(string password)
        {
            string hashedPassword = BC.EnhancedHashPassword(password, 13);

            return hashedPassword;
        }

        public bool VerifyHashPassword(string actualPassword, string hashedPassword)
        {
            return BC.EnhancedVerify(actualPassword, hashedPassword);
        }

        private string GenerateRandomResetPassword()
        {
            Random rand = new Random();
            int number = rand.Next(0, 10000);

            string newRandomResetPassword = $"{_randomPasswordText}-{number}";

            return newRandomResetPassword;
        }

        public string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            string token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }
    }
}