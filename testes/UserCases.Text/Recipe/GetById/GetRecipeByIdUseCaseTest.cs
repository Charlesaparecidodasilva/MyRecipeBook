using AutoMapper;
using Castle.Core.Resource;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggerUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using FluentAssertions;
using MyRacipeBook.Application.UserCases.Recipe.GetById;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using MyRecipeBook.Infrastruture.Services.LoggedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCases.Test.Recipe.GetById
{
    public class GetRecipeByIdUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var user, _ )= UserBuilder.Build();
       
            var recipe = RecipeBuilder.Build(user);

            var useCase =  CreateUseCase(user, recipe);

            var result = await useCase.Execute(recipe.Id);

            result.Should().NotBeNull();
            result.Id.Should().NotBeNullOrWhiteSpace();
            result.Title.Should().Be(recipe.Title);

        }


        [Fact]
        public async Task Error_Recipe_NotFound()
        {
            (var user, _) = UserBuilder.Build();
                  
            var useCase = CreateUseCase(user);

            Func<Task> act = async () => { await useCase.Execute(recipeId: 1000); };

            (await act.Should().ThrowAsync<NotFoundException>())
                .Where(e => e.Message.Equals(ResourceMenssageException.RECIPE_NOT_FOUND));
               
        }

        private static GetRecipeByIdUserCase CreateUseCase           
              ( MyRecipebook.Domain.Entities.User user,
               MyRecipebook.Domain.Entities.Recipe? recipe = null)         
        {
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggerUserBuilder.Build(user);
            var repository = new RecipeReadOnlyRespositoryBuild().GetById(user, recipe).Build();


            return new GetRecipeByIdUserCase(mapper, loggedUser, repository);
        }













    }
}
