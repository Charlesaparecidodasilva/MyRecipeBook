using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipebook.Domain.Repositories.Recipe
{
    public interface IRecipeWriteOnlyRespository
    {
        public Task Add(Entities.Recipe recipe);
        Task Delete(long id);
    }
}
