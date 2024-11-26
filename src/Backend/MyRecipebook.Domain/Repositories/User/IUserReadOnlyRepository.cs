using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyRecipebook;
using System.Threading.Tasks;

namespace MyRecipebook.Domain.Repositories.User
{
    public interface IUserReadOnlyRepository
    {   
        ///Verificando se existe Email igual
        public  Task<bool> ExistActiveUserWhitEmail(string email);

        public  Task<Entities.User?> GetEmailAndPassword(string email, string password);

        public Task<Entities.User?> GetEmailAndName(string email, string name);

        public Task<bool> ExistActiveUserWithIdentifier(Guid userIndentifi);

    }
}











