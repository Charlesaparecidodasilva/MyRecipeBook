using MyRecipeBook.Comunication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.User.Profile
{
    public interface IGetUserProfileUseCase
    {
        Task<ResponseUserProfileJson> Execute();
    }
}
