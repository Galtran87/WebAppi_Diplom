using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace WebAppi_Diplom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string mySecret;
        private readonly string issuer;
        private readonly string audience;

        public LoginController(IConfiguration configuration)
        {
            mySecret = configuration.GetValue<string>("Auth:Secret")!;
            issuer = configuration.GetValue<string>("Auth:Issuer")!;
            audience = configuration.GetValue<string>("Auth:Audience")!;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthModel authModel)
        {
            // Валідація за допомогою Fluent Validation
            AuthModelValidator validator = new AuthModelValidator();
            var validationResult = validator.Validate(authModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            // Логіка авторизації (простий приклад)
            if (IsValidUsername(authModel.Username) && IsValidPassword(authModel.Password))
            {
                // Генерація токена
                var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, authModel.Username),
                    }),
                    Expires = DateTime.UtcNow.AddDays(30),
                    Issuer = issuer,
                    Audience = audience,

                    SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { message = "Успішний вхід", token = tokenString });
            }
            else
            {
                return Unauthorized(new { message = "Не вдалася авторизація. Перевірте ім'я користувача та пароль." });
            }
        }

        private bool IsValidUsername(string username)
        {
            // Регулярний вираз для імені користувача: 
            var usernamePattern = @"^[a-zA-Z0-9]{2,}$";

            return Regex.IsMatch(username, usernamePattern);
        }

        private bool IsValidPassword(string password)
        {
            // Регулярний вираз для пароля:
            var passwordPattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

            return Regex.IsMatch(password, passwordPattern);
        }
    }

    public class AuthModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthModelValidator : AbstractValidator<AuthModel>
    {
        public AuthModelValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Ім'я користувача обов'язкове для заповнення")
                .MinimumLength(2).WithMessage("Ім'я користувача повинно містити щонайменше 2 символи");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обов'язковий для заповнення")
                .MinimumLength(8).WithMessage("Пароль повинен містити щонайменше 8 символів");
        }
    }
}
