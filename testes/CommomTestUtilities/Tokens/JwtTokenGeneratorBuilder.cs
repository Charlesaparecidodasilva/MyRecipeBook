using MyRecipebook.Domain.Security.Tokens;
using MyRecipeBook.Infrastruture.Security.Tokens.Acess.Generator;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Tokens
{
    public class JwtTokenGeneratorBuilder
    {     

        public static IAcessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, siningKey: "wwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww");
    }
}
