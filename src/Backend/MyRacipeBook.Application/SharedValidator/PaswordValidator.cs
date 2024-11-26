using FluentValidation;
using FluentValidation.Validators;
using MyRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.SharedValidator
{
    public class PasswordValidator<T> : PropertyValidator<T, string>
    {     
        public override bool IsValid(ValidationContext<T> context, string password)
        {
           if (string.IsNullOrWhiteSpace(password))
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMenssageException.PASSWORD_EMPATY);
                return false;   
            }

           if (password.Length < 6)
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMenssageException.INVALID_EMAIL_OR_PASSWORD);
                return false;
            }

           return true; 
        }

        public override string Name => "PasswordValidator";

        protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";
        
    }

   
}

