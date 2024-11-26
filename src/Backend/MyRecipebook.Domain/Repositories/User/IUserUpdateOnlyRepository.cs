using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipebook.Domain.Repositories.User
{
    public interface IUserUpdateOnlyRepository
    {
        public Task<Entities.User>GetById(long id);
        public void Update(Entities.User user);
    }
}
