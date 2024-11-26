using Microsoft.IdentityModel.Tokens;
using MyRecipebook.Domain.Security.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastruture.Security.Tokens.Acess.Generator
{
    public class JwtTokenGenerator : JwtTokenHandler, IAcessTokenGenerator
    {
        private readonly uint _expirationTimeMinutes;
        private readonly string _siningKey;

        public JwtTokenGenerator(uint expirationTimeMinutes, string siningKey)
        {
            _expirationTimeMinutes = expirationTimeMinutes;
            _siningKey = siningKey;
        }

        public string Generate(Guid userIndentifier)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, userIndentifier.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
                //A chave de segurança        //Algoritimo de segurança
                SigningCredentials = new SigningCredentials(SecurityKey(_siningKey), SecurityAlgorithms.HmacSha256Signature)
            }; 

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);  
        }


   

    }
}
