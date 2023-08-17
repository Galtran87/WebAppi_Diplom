using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
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
        private readonly string issuer;
        private readonly string audience;

        public LoginController(IConfiguration configuration)
        {
            mySecret = configuration.GetValue<string>("Auth:Secret")!;
            issuer = configuration.GetValue<string>("Auth:Issuer")!;
            audience = configuration.GetValue<string>("Auth:Audience")!;
        }

        [HttpPost]
        public ActionResult<string> GenerateToken([FromBody] LoginModel request)
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
                Issuer = issuer,
                Audience = audience,

                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        [HttpGet]
        public ActionResult<bool> VerifyToken(string token)
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
                    ValidIssuer = issuer,
                    ValidAudience = audience,
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
