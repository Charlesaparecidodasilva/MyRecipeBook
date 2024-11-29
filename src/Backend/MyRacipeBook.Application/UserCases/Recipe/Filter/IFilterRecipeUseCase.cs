using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Comunication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.Recipe.Filter
{
    public interface IFilterRecipeUseCase
    {

        public Task<ResponseRecipesJson> Execulte(RequestFilterRecipeJson request);

    }
}
