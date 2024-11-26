using Microsoft.IdentityModel.Tokens;
using MyRecipebook.Domain.Entities;
using MyRecipebook.Domain.Extensions;
using MyRecipebook.Domain.Repositories;
using MyRecipebook.Domain.Repositories.User;
using MyRecipebook.Domain.Security.Cryptography;
using MyRecipebook.Domain.Services.LoggedUser;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Comunication.Responses;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using MyRecipeBook.Infrastruture.Services.LoggedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.User.ChangePassword
{
    public  class ChangePasswordUserCase : IChangePasswordUserCase
    {

        readonly ILoggedUser _loggedUser;
        readonly IPaswordEncripter _paspwordEncripter;    
        readonly IUserUpdateOnlyRepository _repository;
        readonly IUnitiOfWork _unitiOfWork;

        public ChangePasswordUserCase(
         ILoggedUser loggedUser,
         IPaswordEncripter paspwordEncripter,        
         IUserUpdateOnlyRepository Repository,
         IUnitiOfWork unitiOfWork
          ) 
        {
            _loggedUser = loggedUser;
            _paspwordEncripter = paspwordEncripter;
            _unitiOfWork = unitiOfWork; 
            _repository = Repository;
        }
        public async Task Execult(RequestChangePaswordJson request)
        {
            var loggedUser = await _loggedUser.User();

            Validate(request, loggedUser);

            var user = await _repository.GetById(loggedUser.Id);

            user.Password = _paspwordEncripter.Encrypt(request.NewPassword);

            _repository.Update(user);

            await _unitiOfWork.Commit(); 
        }


        public void Validate(RequestChangePaswordJson request, MyRecipebook.Domain.Entities.User loggedUser)
        {

            var result = new ChangePasswordValidator().Validate(request);

            var currentPasswordEncripted = _paspwordEncripter.Encrypt(request.Password);

            if (currentPasswordEncripted.Equals(loggedUser.Password).IsFalse())
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceMenssageException.PASSWOR_IS_DIFERENT));

            if (result.IsValid.IsFalse())
                throw new ErroOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
        }

    }

}

  
