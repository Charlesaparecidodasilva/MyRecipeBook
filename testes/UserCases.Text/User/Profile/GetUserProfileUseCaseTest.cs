using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggerUser;
using CommomTestUtilities.Mapper;
using FluentAssertions;
using MyRacipeBook.Application.UserCases.User.Profile;
using MyRecipebook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCases.Test.User.Profile
{
    public class GetUserProfileUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, var _) = UserBuilder.Build();

            var userCase = CreateUseCaser(user);

            var result = await userCase.Execute();

            result.Should().NotBeNull();
            result.Name.Should().Be(user.Name);
            result.Email.Should().Be(user.Email);
        }

        private static GetUserProfileUseCase CreateUseCaser(MyRecipebook.Domain.Entities.User user)
        {
            var mapper = MapperBuilder.Build();

            var loggedUser = LoggerUserBuilder.Build(user);

            return new GetUserProfileUseCase(loggedUser , mapper); 
        }
    }
}
