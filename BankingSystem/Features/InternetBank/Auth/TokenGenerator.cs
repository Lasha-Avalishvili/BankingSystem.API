using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BankingSystem.Features.InternetBank.Auth
{
    public class TokenGenerator
    {
        private readonly JwtSettings _settings;
        public TokenGenerator(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }
        public string Generate(string role, string Id) 
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, Id),
                new Claim(ClaimTypes.Role, role),
                new Claim("userId", Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            var tokenGenerator = new JwtSecurityTokenHandler();
            var jwtString = tokenGenerator.WriteToken(token);
            return jwtString;

        }
      
    }
}
