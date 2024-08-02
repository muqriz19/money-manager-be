using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using moneyManagerBE.Class;
using moneyManagerBE.Data;
using moneyManagerBE.Dtos;
using moneyManagerBE.Models;
using moneyManagerBE.Services.Hasher;
using moneyManagerBE.Services.Users;

namespace moneyManagerBE.Services.Authorization
{
    public class AuthorizationService : IAuthorization
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        private readonly string _randomPasswordText = "RANDOM_RESET_PASSWORD";

        private readonly IUsersService _usersService;

        private readonly IHasher _hasher;

        private readonly IConfiguration _config;


        public AuthorizationService(
            AppDbContext appDbContext,
            IMapper mapper,
            IUsersService usersService,
            IHasher hasher,
            IConfiguration config
            )
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _usersService = usersService;
            _hasher = hasher;
            _config = config;
        }

        public DbResponse<LoginResponseDto> Login(string email, string password)
        {
            var foundUser = _usersService.GetUserByEmail(email);

            if (foundUser == null)
            {
                return new DbResponse<LoginResponseDto>
                {
                    IsSuccess = false,
                    Message = "This login does not exist"
                };
            }

            bool isValidLogin = _hasher.VerifyHashPassword(password, foundUser.Password);

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

        public DbResponse<string> ForgotPasswordOperation(string email)
        {
            var foundUser = _usersService.GetUserByEmail(email);

            if (foundUser == null)
            {
                return new DbResponse<string>
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            string resetPassword = _hasher.HashPassword(GenerateRandomResetPassword());
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
            var foundUser = _usersService.GetUserByEmail(email);

            if (foundUser != null)
            {
                foundUser.Password = _hasher.HashPassword(resetPassword.NewPassword);

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

        public DbResponse<bool> CheckResetPasswordHashIsCorrect(string userEmail, string temporaryHashPassword)
        {
            var foundUser = _usersService.GetUserByEmail(userEmail);

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