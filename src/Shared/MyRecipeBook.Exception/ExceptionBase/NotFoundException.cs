using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Exception.ExceptionBase
{
    public class NotFoundException : MyRecipeBookException
    {
        public NotFoundException(string message) : base(message)
        {




        }
    }
}
