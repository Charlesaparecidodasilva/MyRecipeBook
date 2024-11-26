using MyRecipebook.Domain.Security.Cryptography;
using MyRecipeBook.Infrastruture.Security.CryotoGraphy;

namespace CommomTestUtilities.Cryptography
{
    public class PasswordEncripterBuilder
    {
        public static IPaswordEncripter Build() => new Sha512Encripter("CcAa");    

    }
}

