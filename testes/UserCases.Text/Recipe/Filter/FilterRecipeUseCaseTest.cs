using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggerUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Request;
using FluentAssertions;
using MyRacipeBook.Application.UserCases.Recipe.Filter;
using MyRecipebook.Domain.Repositories.Recipe;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCases.Test.Recipe.Filter
{
    public class FilterRecipeUseCaseTest
    {

        [Fact]//////////////
        public async Task  Success()
        {
            (var user, _) = UserBuilder.Build();

            var request = RequestFilterRecipeJsonBuilder.Build();

            var recipes = RecipeBuilder.Collection(user);

            var useCase = CreateUseCase(user, recipes);

            var result = await useCase.Execulte(request);

            result.Should().NotBeNull();
            result.Recipes.Should().NotBeNullOrEmpty();
            result.Recipes.Should().HaveCount(recipes.Count);
        }

        [Fact]
        public async Task Error_CookingTime_Invalid()
        {
            (var user, _) = UserBuilder.Build();

            var recipe = RecipeBuilder.Collection(user);

            var request = RequestFilterRecipeJsonBuilder.Build();

            request.CookingTimes.Add((MyRecipebook.Comunication.Enum.CookingTime)1000);

            var useCase = CreateUseCase(user, recipe);

            Func<Task> act = async () => { await useCase.Execulte(request); };

            (await act.Should().ThrowAsync<ErroOnValidationException>())
                .Where(e => e.MenssageErroValidada.Contains(ResourceMenssageException.COOKING_TIME_NOT_SUPORTED));
        }



        private static FilterRecipeUseCase CreateUseCase(
            MyRecipebook.Domain.Entities.User user,
            IList<MyRecipebook.Domain.Entities.Recipe> recipe
            )
        {
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggerUserBuilder.Build(user);
            var repository = new RecipeReadOnlyRespositoryBuild().Filter(user, recipe).Build();
            return new FilterRecipeUseCase(loggedUser, mapper, repository); 
        }       
    }
}
