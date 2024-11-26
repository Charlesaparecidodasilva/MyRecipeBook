using Microsoft.EntityFrameworkCore;
using MyRecipebook.Domain.Entities;
using MyRecipebook.Domain.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastruture.DataAcess.Repositories
{
  
                               ///Devo iplementar as interfaces Write e Read aqui/
    public class UserRepository : IUserWriteOnlyRepository , IUserReadOnlyRepository , IUserUpdateOnlyRepository
    {
        private readonly MyRecipeBookDbContext _dbContext;
        
        public UserRepository(MyRecipeBookDbContext dbContext) => _dbContext = dbContext;


        ///Nessa funçao srá passada o usuario para o banco de dados.(Essa função é invocada em IUserWriteOnlyRepository na pasta de repositories.
        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }     
        //Aqui verifico se algum usuario do _dbContext(banco de dados) tem o email igual ao email que sera passado, e se ele é ativo(Essa função é invocada em IUserReadOnlyRepository na pasta de repositories..
        public async Task<bool> ExistActiveUserWhitEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);
        
        public async Task<User?> GetEmailAndPassword(string email, string pasword)
        {
            return await _dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email) && user.Password.Equals(pasword));         
        }

        public async Task<User?> GetEmailAndName(string email, string name)
        {
            return await _dbContext
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email) && user.Name.Equals(name));
        }

        public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifi) => await _dbContext.Users.AnyAsync(user => user.UserIdentifier.Equals(userIdentifi) && user.Active);

        public async  Task<User> GetByUserIdentifier(Guid userIdentifi)
        {
            return await _dbContext
                .Users
                .AsNoTracking()
                .FirstAsync(user => user.Active && user.UserIdentifier.Equals(userIdentifi));
        }
      
        public async Task<User> GetById(long id)
        {
            return await _dbContext
                .Users
                .FirstAsync(user => user.Id == id);
        }

        public void Update(User user)
        {
            _dbContext.Users.Update(user);
        }
    }
}

