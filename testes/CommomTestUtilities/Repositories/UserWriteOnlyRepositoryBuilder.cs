using Moq;
using MyRecipebook.Domain.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Repositories
{
    public class UserWriteOnlyRepositoryBuilder
    {
        public static  IUserWriteOnlyRepository Build()
        {
            var mock = new Mock<IUserWriteOnlyRepository>();
            
            return mock.Object;
        }
    }
}
