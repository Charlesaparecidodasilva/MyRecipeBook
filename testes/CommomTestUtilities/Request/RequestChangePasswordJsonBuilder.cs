using Bogus;
using MyRecipeBook.Comunication.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Request
{
    public class RequestChangePasswordJsonBuilder
    {

        public static RequestChangePaswordJson Build(int passwordLength = 10)
        {
            return new Faker<RequestChangePaswordJson>()
                .RuleFor(u => u.Password, (f) => f.Internet.Password())
                .RuleFor(u => u.NewPassword, (f) => f.Internet.Password(passwordLength));
        }
    }
}
