using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipebook.Domain.Security.Cryptography
{
    public interface IPaswordEncripter
    {

        public string Encrypt(string password);

    }
}
