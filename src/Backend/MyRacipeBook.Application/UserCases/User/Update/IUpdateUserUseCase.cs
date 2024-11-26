using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Comunication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.User.Update
{
    public interface IUpdateUserUseCase
    {
        public  Task Execute(RequestUpdateUserJson request);      
    }
}
