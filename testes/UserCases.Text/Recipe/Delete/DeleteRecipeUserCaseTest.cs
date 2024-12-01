using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggerUser;
using CommomTestUtilities.Repositories;
using FluentAssertions;
using MyRacipeBook.Application.UserCases.Recipe.Delete;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using MyRecipeBook.Infrastruture.Services.LoggedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCases.Test.Recipe.Delete
{
    public class DeleteRecipeUserCaseTest
    {

        [Fact]
        public async Task Sucess()
        {
            (var user, _) = UserBuilder.Build();

            var recipe = RecipeBuilder.Build(user);

            var useCase = CreateUserCase(user, recipe);

            Func<Task> act = async () => { await useCase.Execute(recipe.Id); };

            await act.Should().NotThrowAsync();
        }


        [Fact]
        public async Task Error_Recipe_NotFound()
        {
            (var user, _) = UserBuilder.Build();

            var useCase = CreateUserCase(user);

            Func<Task> act = async () => { await useCase.Execute(1); };

            await act.Should().ThrowAsync<NotFoundException>()
               .Where(e => e.Message.Equals(ResourceMenssageException.RECIPE_NOT_FOUND));

        }


        private  DeleteRecipeUserCase CreateUserCase
            (MyRecipebook.Domain.Entities.User user,
             MyRecipebook.Domain.Entities.Recipe? recipe = null
            )
        {
            var userlogg = LoggerUserBuilder.Build(user);
            var repositoryRead = new RecipeReadOnlyRespositoryBuild().GetById(user, recipe).Build();
            var repositoryWrite = RecipeWriteOnlyRespositoryBuild.Build();
            var uniteOfWork = UnitiOfWorkBuilder.Build();

            return new DeleteRecipeUserCase(userlogg, repositoryWrite, repositoryRead, uniteOfWork);
        }


    }
}
