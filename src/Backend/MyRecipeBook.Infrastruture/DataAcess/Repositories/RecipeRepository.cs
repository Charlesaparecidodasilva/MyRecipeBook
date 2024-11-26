using Microsoft.EntityFrameworkCore;
using MyRecipebook.Domain.Dtos;
using MyRecipebook.Domain.Entities;
using MyRecipebook.Domain.Extensions;
using MyRecipebook.Domain.Repositories.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastruture.DataAcess.Repositories
{
    public class RecipeRepository : IRecipeWriteOnlyRespository , IRecipeReadOnlyRespository
    {

        public readonly MyRecipeBookDbContext _dbContext;
        public RecipeRepository(MyRecipeBookDbContext dbContext) => _dbContext = dbContext; 

        public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);


        public async Task<IList<Recipe>> Filter(User user, FilterRecipesDto filters)
        {

            var query = _dbContext
                        .Recipes.AsNoTracking()
                        .Include(recipe => recipe.Ingredients)
                        .Where(recipe => recipe.Active && recipe.UserId == user.Id);

            if (filters.Difficulties.Any())
            {
               query = query.Where(recipe => recipe.Difficulty.HasValue && filters.Difficulties.Contains(recipe.Difficulty.Value));
               
            }
            if (filters.CookingTimes.Any())
            {
                query = query.Where(recipe => recipe.CookingTime.HasValue && filters.CookingTimes.Contains(recipe.CookingTime.Value));
            }

            if (filters.DishTypes.Any())
            {                            //No DishTypes, existe algum cujo o Type é igua l ao do filtro
                query = query.Where(recipe => recipe.DishTypes.Any(dishTypes => filters.DishTypes.Contains(dishTypes.Type)));
            }

            if (filters.RecipeTitle_Ingredient.NotEmpaty())
            {
                query = query.Where(

                    // ou chma pela receita ou pelo ingrediente.
                    recipe => recipe.Title.Contains(filters.RecipeTitle_Ingredient)                   
                    || recipe.Ingredients.Any(ingredientes => ingredientes.Item.Contains(filters.RecipeTitle_Ingredient)));

            }
            return await query.ToListAsync();
        }
    }
}

