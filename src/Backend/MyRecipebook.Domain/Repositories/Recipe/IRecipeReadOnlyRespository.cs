using MyRecipebook.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipebook.Domain.Repositories.Recipe
{
    public interface IRecipeReadOnlyRespository
    {

        Task<IList<Entities.Recipe>> Filter(Entities.User user, FilterRecipesDto filters);


        Task<Entities.Recipe?> GetById(Entities.User user,  long recipeId);



    }
}
