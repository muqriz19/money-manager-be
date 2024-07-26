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

        public DbResponse<LoginResponseDto> Login(string email, string password)
        {
            var foundUser = GetUserByEmail(email);

            if (foundUser == null)
            {
                return new DbResponse<LoginResponseDto>
                {
                    IsSuccess = false,
                    Message = "This login does not exist"
                };
            }

            bool isValidLogin = VerifyHashPassword(password, foundUser.Password);

            if (!isValidLogin)
            {
                return new DbResponse<LoginResponseDto>
                {
                    IsSuccess = false,
                    Message = "Invalid login"
                };
            }

            return new DbResponse<LoginResponseDto>
            {
                IsSuccess = true,
                Message = "Valid login",
                Data = _mapper.Map<LoginResponseDto>(foundUser)
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

        public DbResponse<string> ForgotPasswordOperation(string email)
        {
            var foundUser = GetUserByEmail(email);

            if (foundUser == null)
            {
                return new DbResponse<string>
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            string resetPassword = HashPassword(GenerateRandomResetPassword());
            foundUser.Password = resetPassword;

            _appDbContext.Users.Update(foundUser);
            _appDbContext.SaveChanges();

            return new DbResponse<string>
            {
                IsSuccess = true,
                Message = "Initialising...",
                Data = resetPassword
            };
        }

        public DbResponse<bool> ChangePassword(string email, ResetPassword resetPassword)
        {
            var foundUser = GetUserByEmail(email);

            if (foundUser != null)
            {
                foundUser.Password = HashPassword(resetPassword.NewPassword);

                _appDbContext.Users.Update(foundUser);
                _appDbContext.SaveChanges();

                return new DbResponse<bool>
                {
                    IsSuccess = true,
                    Message = "Password successfully changed"
                };
            }
            else
            {
                return new DbResponse<bool>
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }
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

        public DbResponse<bool> CheckResetPasswordHashIsCorrect(string userEmail, string temporaryHashPassword)
        {
            var foundUser = GetUserByEmail(userEmail);

            if (foundUser == null)
            {
                return new DbResponse<bool>
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            bool isHashPasswordCorrect = foundUser.Password == temporaryHashPassword;
            return new DbResponse<bool>
            {
                IsSuccess = isHashPasswordCorrect,
                Message = isHashPasswordCorrect ? "Hash password same" : "Hash password not the same"
            };
        }
    }
}