using MyRecipebook.Domain.Repositories;
using MyRecipebook.Domain.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastruture.DataAcess
{
    public class UnitiOfWork : IUnitiOfWork
    {
        private readonly MyRecipeBookDbContext _dbContext;

        public UnitiOfWork(MyRecipeBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Método Commit que salva as alterações no banco de dados de forma assíncrona
        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}


