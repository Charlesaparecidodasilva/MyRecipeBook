using CommomTestUtilities.Repositories;
using Moq;
using MyRecipebook.Domain.Entities;
using MyRecipebook.Domain.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Repositories
{
    public class UserUpdateOnlyRepositoryBuild
    {
        private readonly Mock<IUserUpdateOnlyRepository> _mockRepository;

        public UserUpdateOnlyRepositoryBuild() => _mockRepository = new Mock<IUserUpdateOnlyRepository>();
       
        
        public UserUpdateOnlyRepositoryBuild GetById ( User user)
        {
            _mockRepository.Setup(x => x.GetById(user.Id)).ReturnsAsync(user);
            return this;
        }

        public UserUpdateOnlyRepositoryBuild Update(User user) 
        {
            _mockRepository.Setup(repo => repo.Update(user));
            return this;
        }
        public IUserUpdateOnlyRepository Build() => _mockRepository.Object;
    }
}
