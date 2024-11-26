using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastruture.Security.Tokens.Acess
{
    public abstract class JwtTokenHandler
    {


        //Nessa função a chave de segurança _siningKey é convertida de string para uma symetric Security.
        protected SymmetricSecurityKey SecurityKey(string siningKey)
        {
            var bytes = Encoding.UTF8.GetBytes(siningKey);

            return new SymmetricSecurityKey(bytes);
        }
    }
}
