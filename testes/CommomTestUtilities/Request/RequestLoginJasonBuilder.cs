using Bogus;
using MyRecipeBook.Comunication.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Request
{
    public class RequestLoginJasonBuilder
    {

        public static RequestLoguinJason Build()
        {
            return new Faker<RequestLoguinJason>()
                .RuleFor(u => u.Email, (f) => f.Internet.Email())
                .RuleFor(u => u.Password, (f) => f.Internet.Password());         
        }
    }
}
