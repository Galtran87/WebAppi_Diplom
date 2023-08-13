using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;  // Додано простір імен для IConfiguration
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAppi_Diplom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string mySecret;
        private readonly string myIssuer;     // Додано поле для збереження інформації з конфігурації
        private readonly string myAudience;   // Додано поле для збереження інформації з конфігурації

        public LoginController(IConfiguration configuration)
        {
            mySecret = configuration.GetValue<string>("Auth:Secret")!;
            myIssuer = configuration.GetValue<string>("Auth:Issuer")!;      // Вичитання значення myIssuer з конфігурації
            myAudience = configuration.GetValue<string>("Auth:Audience")!;  // Вичитання значення myAudience з конфігурації
        }

        [HttpPost]
        public string GenerateToken([FromBody] LoginModel request)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, request.Username),
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                Issuer = myIssuer,      // Використання значення myIssuer
                Audience = myAudience,  // Використання значення myAudience

                SigningCredentials = new SigningCredentials(mySecurityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpGet]
        public bool VerifyToken(string token)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = myIssuer,      // Використання значення myIssuer
                    ValidAudience = myAudience,  // Використання значення myAudience
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
