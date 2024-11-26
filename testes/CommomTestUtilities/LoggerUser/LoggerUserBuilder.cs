using Moq;
using MyRecipebook.Domain.Entities;
using MyRecipebook.Domain.Services.LoggedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.LoggerUser
{
    public class LoggerUserBuilder
    {
        public static ILoggedUser Build(User user)
        {
            var mock = new Mock<ILoggedUser>();
            mock.Setup(x => x.User()).ReturnsAsync(user);

            return mock.Object;
        }
    }
}

