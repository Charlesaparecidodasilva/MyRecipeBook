using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Exception.ExceptionBase
{
    public class InvalidLoginException : MyRecipeBookException
    {
        public InvalidLoginException() : base(ResourceMenssageException.INVALID_EMAIL_OR_PASSWORD)
        {

        }
    }
}
