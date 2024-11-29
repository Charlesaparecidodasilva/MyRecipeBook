using AutoMapper;
using MyRecipebook.Domain.Repositories.Recipe;
using MyRecipebook.Domain.Services.LoggedUser;
using MyRecipeBook.Comunication.Responses;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.Recipe.GetById
{
    public class GetRecipeByIdUserCase : IGetRecipeByIdUserCase
    {
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;
        private readonly IRecipeReadOnlyRespository _repository;

        public GetRecipeByIdUserCase(
            IMapper  mapper,
            ILoggedUser loggedUser,
            IRecipeReadOnlyRespository repository                       
            )
        {
            _mapper = mapper;
            _loggedUser = loggedUser;  
            _repository = repository;
        }

       public async Task<ResponseRecipeJson> Execute( long recipeId)
        {
            var loggedUser = await _loggedUser.User();

            var recipe = await _repository.GetById(loggedUser, recipeId);

            if (recipe is null)
            {
                throw new NotFoundException(ResourceMenssageException.RECIPE_NOT_FOUND);
            }
            return _mapper.Map<ResponseRecipeJson>(recipe);
        }














    }
}
