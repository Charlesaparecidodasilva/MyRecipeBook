using AutoMapper;
using MyRecipebook.Domain.Extensions;
using MyRecipebook.Domain.Repositories;
using MyRecipebook.Domain.Repositories.Recipe;
using MyRecipebook.Domain.Services.LoggedUser;
using MyRecipeBook.Comunication.Request;
using MyRecipebook.Domain.Entities;
using MyRecipeBook.Comunication.Responses;
using MyRecipeBook.Exception.ExceptionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.Recipe
{
    public class RegisterRecipeUseCase : IRegisteRecipeUseCase
    {
        private readonly IRecipeWriteOnlyRespository _respository;
        private readonly ILoggedUser _loggedUser;
        private readonly IUnitiOfWork _unitiOfWork;
        private readonly IMapper _mapper;

        public RegisterRecipeUseCase(
            IRecipeWriteOnlyRespository repository,
            ILoggedUser loggedUser,
            IUnitiOfWork unitiOfWork,
            IMapper mapper 
            )

        {
            _respository = repository;
            _loggedUser = loggedUser;
            _unitiOfWork = unitiOfWork;
            _mapper = mapper;           
        }       

        public async Task<ResponseRegisteredRecipeJason> Execute(RequestRecipeJson request)
        {           
            
                Validate(request);

                var loogUser = await _loggedUser.User();

                var recipe = _mapper.Map<MyRecipebook.Domain.Entities.Recipe>(request);

                recipe.UserId = loogUser.Id;

                var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
                for (var index = 0; index < instructions.Count; index++)
                    instructions.ElementAt(index).Step = index + 1;

                recipe.Instructions = _mapper.Map<IList<MyRecipebook.Domain.Entities.Instruction>>(instructions);

                await _respository.Add(recipe);
                await _unitiOfWork.Commit();

                return _mapper.Map<ResponseRegisteredRecipeJason>(recipe);

            
        }

        private static void Validate(RequestRecipeJson request)
        {
            var result = new RecipeValidator().Validate(request);

            if (result.IsValid.IsFalse())
            {
                throw new ErroOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
            }
        }


    }
}
