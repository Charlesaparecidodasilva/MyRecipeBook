using Microsoft.EntityFrameworkCore;
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

        public RecipeReadOnlyRespositoryBuild GetById(User user, Recipe? recipe)
        {
            // Verifica se a receita passada não é nula.
            if (recipe is not null)
            {
                // Configura o comportamento do mock do repositório.
                // Define que quando o método GetById for chamado com o usuário e o ID da receita,
                // o mock retornará a receita fornecida.
                _respository.Setup(repository => repository.GetById(user, recipe.Id)).ReturnsAsync(recipe); // Retorna a receita de forma assíncrona.
            }

            // Retorna a própria instância da classe para permitir chamadas encadeadas (pattern Fluent Interface).
            return this;
        }

        public IRecipeReadOnlyRespository Build() => _respository.Object;

    }
}
