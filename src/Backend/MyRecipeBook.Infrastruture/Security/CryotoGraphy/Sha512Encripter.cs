using MyRecipebook.Domain.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastruture.Security.CryotoGraphy
{
    public class Sha512Encripter : IPaswordEncripter
    {
      
            private readonly string _additionaliKey;
            public Sha512Encripter(string additionaliKey) => _additionaliKey = additionaliKey;


            public string Encrypt(string Password)
            {
                var newPassword = $"{Password}{_additionaliKey}";

                var bytes = Encoding.UTF8.GetBytes(newPassword);
                var hashBytes = SHA512.HashData(bytes);

                return StringBytes(hashBytes);
            }

            private static string StringBytes(byte[] bytes)
            {
                var sb = new StringBuilder();
                foreach (byte b in bytes)
                {
                    var hex = b.ToString("x2");
                    sb.Append(hex);
                }

                return sb.ToString();
            }
        

    }
}
