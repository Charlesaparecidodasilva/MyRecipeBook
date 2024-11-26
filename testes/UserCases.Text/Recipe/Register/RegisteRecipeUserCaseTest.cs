using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggerUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Request;
using FluentAssertions;
using MyRacipeBook.Application.UserCases.Recipe;
using MyRecipebook.Domain.Repositories.Recipe;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCases.Test.Recipe.Register
{
    public class RegisteRecipeUserCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _) = UserBuilder.Build();
            var request = RequestRecipeJsonBuilder.Build();
            var useCase = CreateUserCase(user);

            var result = await useCase.Execute(request);
            
            result.Should().NotBeNull();
            result.Id.Should().NotBeNullOrWhiteSpace();
            result.Title.Should().Be(request.Title);
        }

        [Fact]
        public async Task Error_Title_Empaty()
        {
            (var user, _) = UserBuilder.Build();
            var request = RequestRecipeJsonBuilder.Build();
            
            request.Title = string.Empty;

            var useCase = CreateUserCase(user);

            Func<Task> act = async () => { await useCase.Execute(request); };

            (await act.Should().ThrowAsync<ErroOnValidationException>())
                .Where(e => e.MenssageErroValidada.Count == 1 &&
                    e.MenssageErroValidada.Contains(ResourceMenssageException.RECIPY_TITLE_EMPTY));
        }


        private static RegisterRecipeUseCase CreateUserCase(MyRecipebook.Domain.Entities.User user)
        {
            var mapper = MapperBuilder.Build();
            var unitOfWork = UnitiOfWorkBuilder.Build();
            var loogUser = LoggerUserBuilder.Build(user);
            var repository = RecipeWriteOnlyRespositoryBuild.Build();        
            return new RegisterRecipeUseCase( repository, loogUser, unitOfWork, mapper);
        }


    }
}
