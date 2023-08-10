using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public LoginController(IConfiguration configuration)
        {
            mySecret = configuration.GetValue<string>("Auth:Secret")!;
        }
        
        [HttpPost]
        public string GenerateToken([FromBody] LoginModel request)
        {
            //https://dotnetcoretutorials.com/creating-and-validating-jwt-tokens-in-asp-net-core/?expand_article=1

            //var mySecret = "tralala123yljyljy";
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var myIssuer = "http://mysite.com";
            var myAudience = "http://myaudience.com";

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, request.Username),
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                Issuer = myIssuer,
                Audience = myAudience,
                
                SigningCredentials = new SigningCredentials(mySecurityKey,
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpGet]
        public bool VerifyToken(string token)
        {
            //var mySecret = "tralala123yljyljy";
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var myIssuer = "http://mysite.com";
            var myAudience = "http://myaudience.com";

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
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


}
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
       

        
    }

