using Azure.Core;
using MyRecipebook.Domain.Extensions;
using MyRecipebook.Domain.Repositories;
using MyRecipebook.Domain.Repositories.User;
using MyRecipebook.Domain.Services.LoggedUser;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using MyRecipeBook.Infrastruture.Services.LoggedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.User.Update
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IUserReadOnlyRepository _readRepository;
        private readonly IUserUpdateOnlyRepository _repository;
        private readonly IUnitiOfWork _unitOfWork;
    

       public UpdateUserUseCase(
        IUserReadOnlyRepository readOnlyRepository,
        IUserUpdateOnlyRepository repository,
        ILoggedUser loggedUser,
        IUnitiOfWork unitiOfWork)
        {
            _loggedUser = loggedUser;
            _readRepository = readOnlyRepository;
            _repository = repository;
            _unitOfWork = unitiOfWork;
        }
        public async Task Execute(RequestUpdateUserJson request)
        {
            var loggedUser = await _loggedUser.User();

            await Validate(request, loggedUser.Email);

            var user = await _repository.GetById(loggedUser.Id);

            user.Name = request.Name;   
           
            user.Email = request.Email;

            _repository.Update(user);

            await _unitOfWork.Commit();
        }

        private async Task Validate(RequestUpdateUserJson request, string currentEemail)
        {
            var validator = new  UpdateUserValidation();

            var result = validator.Validate(request);

            if (currentEemail.Equals(request.Email).IsFalse())
            {
                var  userExiste = await _readRepository.ExistActiveUserWhitEmail(request.Email);
                if (userExiste)
                    result.Errors.Add(new FluentValidation.Results.ValidationFailure("email", ResourceMenssageException.EMAIL_ALREADY_REGISTERED));
            }
            if (result.IsValid.IsFalse())
            {
                var errorMessage = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErroOnValidationException(errorMessage);
            }
        }
    }
}

