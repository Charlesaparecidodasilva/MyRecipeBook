using Microsoft.IdentityModel.Tokens;
using MyRecipebook.Domain.Security.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastruture.Security.Tokens.Acess.Validator
{
    public class JwtTokenValidator : JwtTokenHandler, IAccessTokenValidator
    {
        private readonly string _siningKey;
     
        public JwtTokenValidator(string siningKey) => _siningKey = siningKey;

        public Guid ValidateAndGetUserIdentifier(string token)
        {
            var validateParameter = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = SecurityKey(_siningKey),
                ClockSkew = new TimeSpan(0)
            };      
            var tokenHandler = new JwtSecurityTokenHandler();

            var principal  = tokenHandler.ValidateToken(token, validateParameter, out _);

            var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            return Guid.Parse(userIdentifier);
        }
    }
}
