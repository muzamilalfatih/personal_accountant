using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using personal_accountant.DTOs;
using personal_accountant.Services.Interfaces;
using personal_accountant.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace personal_accountant.Services
{
    public class TokenService : ITokenServiceInterface
    {
        
        private readonly JwtOptions _jwtOptions;

        public TokenService( JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }
        public string GenerateToken(UserDTO user)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey)), 
                SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new (ClaimTypes.Email , user.Email),
                    new(ClaimTypes.Role , user.Role)
                })
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
            
        }
        public  string GenerateResetToken(int length = 32)
        {
            var bytes = RandomNumberGenerator.GetBytes(length);
            return WebEncoders.Base64UrlEncode(bytes);
        }
    }
}
