using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    // Service class to create JWT token
    public class TokenService
    {
        // Injecting config to get access to the token key
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        // Method that creates the JWT using a user
        public string CreateToken(AppUser user)
        {
            // Specifiying claims that will be included in the token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            // Creating encrypted credentials using a key from config
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Creating token decriptor that describes the claims, expiry and credentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            // Creating the token using the decriptor and a JWT token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return created token
            return tokenHandler.WriteToken(token);
        }
    }
}