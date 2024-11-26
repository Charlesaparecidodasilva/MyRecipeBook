using FluentValidation;
using MyRacipeBook.Application.SharedValidator;
using MyRecipebook.Domain.Extensions;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.User.Register
{
    public class RegisteUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisteUserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMenssageException.NAME_EMPATY);
            RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMenssageException.EMAIL_EMPATY);
            RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
            When(user => string.IsNullOrEmpty(user.Email).IsFalse(), () =>
            {
                RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMenssageException.EMAIL_INVALID);
            });
        }
    }
}
