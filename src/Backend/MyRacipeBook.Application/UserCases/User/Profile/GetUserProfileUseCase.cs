using AutoMapper;
using Microsoft.Extensions.Logging;
using MyRecipebook.Domain.Services.LoggedUser;
using MyRecipeBook.Comunication.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.User.Profile
{
    public class GetUserProfileUseCase : IGetUserProfileUseCase
    {
        private readonly ILoggedUser _loggedUser;

        private readonly IMapper _mapper;
        public GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper)
        {
            _loggedUser = loggedUser;
            _mapper = mapper;
        }

        public async Task<ResponseUserProfileJson> Execute()
        {

            var user = await _loggedUser.User();

            return _mapper.Map<ResponseUserProfileJson>(user);

        }
    }

}
