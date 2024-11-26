using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Comunication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.User.Register
{
    public interface IRegisterUserUseCase
    {

        // A request é jason com os dados enviado pelo usuario.
        public Task<ResponseRegisteredUserJason> Execute(RequestRegisterUserJson request);

    }  
}
