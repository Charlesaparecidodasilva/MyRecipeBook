using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Comunication.Responses
{
    public class ResponseErrorJason
    {
        public IList<string> Errors { get; set; }

        public ResponseErrorJason(IList<string> errors) => Errors = errors;
                      
        public bool TokenIsExired { get; set; } 

        public ResponseErrorJason(string errors)
        {
            Errors = new List<string>
            {
                errors
            };

        }
    }
}
