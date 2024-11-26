using Azure.Core;
using FluentValidation;
using MyRecipebook.Domain.Extensions;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.User.Update
{
    public class UpdateUserValidation : AbstractValidator<RequestUpdateUserJson>
    {

        public UpdateUserValidation()
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceMenssageException.NAME_EMPATY);
            RuleFor(request => request.Email).NotEmpty().WithMessage(ResourceMenssageException.EMAIL_EMPATY);

            When(request => string.IsNullOrWhiteSpace(request.Email).IsFalse(), () =>
            {

                RuleFor(request => request.Email).EmailAddress().WithMessage(ResourceMenssageException.EMAIL_INVALID);
            });
        }


    }
}
