using FluentValidation;
using MyRacipeBook.Application.SharedValidator;
using MyRecipeBook.Comunication.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.User.ChangePassword
{
    public class ChangePasswordValidator : AbstractValidator<RequestChangePaswordJson>
    {
        public ChangePasswordValidator()
        {

            RuleFor(x => x.NewPassword).SetValidator(new PasswordValidator<RequestChangePaswordJson>());

        } 
    }
}

