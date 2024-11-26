using MyRecipeBook.Comunication.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.User.ChangePassword
{
    public interface IChangePasswordUserCase
    {
        public Task Execult(RequestChangePaswordJson request);
    }
}
