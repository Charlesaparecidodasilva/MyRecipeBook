using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.Recipe.Delete
{
    public interface IDeleteRecipeUserCase
    {
        public Task Execute(long recipeId);      
    }
}
