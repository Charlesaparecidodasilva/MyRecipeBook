using Moq;
using MyRecipebook.Domain.Dtos;
using MyRecipebook.Domain.Entities;
using MyRecipebook.Domain.Repositories.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Repositories
{
    public class RecipeReadOnlyRespositoryBuild
    {

        private readonly Mock<IRecipeReadOnlyRespository> _respository;

        public RecipeReadOnlyRespositoryBuild() => _respository = new Mock<IRecipeReadOnlyRespository>();

        public RecipeReadOnlyRespositoryBuild Filter(User user, IList<Recipe> recipes)
        {
            _respository.Setup(repository => repository.Filter(user, It.IsAny<FilterRecipesDto>())).ReturnsAsync(recipes);

            return this;
        }

        public IRecipeReadOnlyRespository Build() => _respository.Object;

    }
}
