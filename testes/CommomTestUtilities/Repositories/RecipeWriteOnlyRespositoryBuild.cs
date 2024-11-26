using Moq;
using MyRecipebook.Domain.Repositories.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Repositories
{
    public class RecipeWriteOnlyRespositoryBuild
    {

        public static IRecipeWriteOnlyRespository Build ()
        {
            var mock = new Mock<IRecipeWriteOnlyRespository>();
            return mock.Object;
        }




    }
}
