using Bogus;
using Moq;
using MyRecipebook.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Repositories
{
    public class UnitiOfWorkBuilder
    {

        public static IUnitiOfWork Build()
        {
            var fork = new Mock<IUnitiOfWork>();

            return fork.Object;

        }

    }
}
