using Castle.Core.Resource;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggerUser;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Request;
using FluentAssertions;
using MyRacipeBook.Application.UserCases.User.Update;
using MyRecipebook.Domain.Entities;
using MyRecipebook.Domain.Extensions;
using MyRecipebook.Domain.Repositories.User;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCases.Test.User.Update
{
    public class UpdateUserUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();          

            var useCase = CreatUserCase(user);

            Func<Task> act = async () => await useCase.Execute(request);

            await act.Should().NotThrowAsync();

            user.Name.Should().Be(request.Name);
            user.Email.Should().Be(request.Email);

        }

        [Fact]

        public async Task Erro_Name_Empaty()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestUpdateUserJsonBuilder.Build();

            request.Name = string.Empty;

            var userCase = CreatUserCase(user);

            Func<Task> act = async () => { await userCase.Execute(request); };

            (await act.Should().ThrowAsync<ErroOnValidationException>())
                .Where(e => e.MenssageErroValidada.Count == 1 &&
                 e.MenssageErroValidada.Contains(ResourceMenssageException.NAME_EMPATY));

            user.Name.Should().NotBe(request.Name);
            user.Email.Should().NotBe(request.Name);

        }

        [Fact]
        public async Task Error_Email_Alredy_Registered()
        {
            (var user, _) = UserBuilder.Build();
            var request = RequestUpdateUserJsonBuilder.Build();
            var userCase = CreatUserCase(user, request.Email);

            Func<Task> act = async () => { await userCase.Execute(request); };

            (await act.Should().ThrowAsync<ErroOnValidationException>())
                .Where(e => e.MenssageErroValidada.Count == 1 &&
                e.MenssageErroValidada.Contains(ResourceMenssageException.EMAIL_ALREADY_REGISTERED));
            
            user.Name.Should().NotBe(request.Name);
            user.Email.Should().NotBe(request.Email);


        }
        private static UpdateUserUseCase CreatUserCase(MyRecipebook.Domain.Entities.User user, string ? email = null)
        {
            var unitOfwork = UnitiOfWorkBuilder.Build();
            var userUpdateRepository = new UserUpdateOnlyRepositoryBuild().GetById(user).Build();
            var userLogger = LoggerUserBuilder.Build(user);
   
            var userReadOnlyRepositoryBuild = new UserReadOnlyRepositoryBuilder();
            if (string.IsNullOrEmpty(email).IsFalse())
                userReadOnlyRepositoryBuild.ExistActiveUserWhitEmail(email !);
       
            return new UpdateUserUseCase(userReadOnlyRepositoryBuild.Build(), userUpdateRepository, userLogger, unitOfwork);
        }
    }
}

