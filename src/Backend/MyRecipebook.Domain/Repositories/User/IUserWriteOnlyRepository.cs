using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipebook.Domain.Repositories.User
{
    public interface IUserWriteOnlyRepository
    {
        //Aqui registro o usuario
        public  Task Add(Entities.User user);      
    }
}
