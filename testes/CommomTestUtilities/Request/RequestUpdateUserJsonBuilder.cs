using Bogus;
using MyRecipebook.Domain.Entities;
using MyRecipeBook.Comunication.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Request
{
    public class RequestUpdateUserJsonBuilder
    {

        public static RequestUpdateUserJson Build()
        {

            return new Faker<RequestUpdateUserJson>().
                RuleFor(user => user.Name, (f) => f.Person.FirstName).
                RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name));
        }

    }
}
