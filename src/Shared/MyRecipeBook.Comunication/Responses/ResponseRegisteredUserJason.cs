using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Comunication.Responses
{
    public class ResponseRegisteredUserJason
    {
       public string Name { get; set; } = string.Empty;

        public ResponseTokensJson Tokens { get; set; } = default!;
    }
}











