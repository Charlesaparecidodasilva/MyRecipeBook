using Bogus;
using CommomTestUtilities.Cryptography;
using MyRecipebook.Domain.Entities;


namespace CommomTestUtilities.Entities
{
    public class UserBuilder
    {     
        public static (User user, string passwod) Build()
        {
            var passwordEncript = PasswordEncripterBuilder.Build();

            var password = new Faker().Internet.Password();     
            
            var user = new Faker<User>()
              .RuleFor(user => user.Id, () => 1)
              .RuleFor(user => user.Name, (f) => f.Person.FirstName)
              .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Email))
              .RuleFor(user => user.UserIdentifier, _=> Guid.NewGuid())
              .RuleFor(user => user.Password, (f, user) => passwordEncript.Encrypt(password));

            return (user, password);
        }
    }
}

