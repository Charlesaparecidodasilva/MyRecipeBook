using FluentValidation;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRacipeBook.Application.UserCases.Recipe.Filter
{
    public class FilterRecipeValidator : AbstractValidator<RequestFilterRecipeJson>
    {
        public FilterRecipeValidator()
        {
            RuleForEach(r => r.CookingTimes).IsInEnum().WithMessage(ResourceMenssageException.COOKING_TIME_NOT_SUPORTED);
            RuleForEach(r => r.Difficulties).IsInEnum().WithMessage(ResourceMenssageException.DIFUCULT_LEVEL_NOT_SUPORT);
            RuleForEach(r => r.DishTypes).IsInEnum().WithMessage(ResourceMenssageException.DISH_TYPE_NOT_SUPORTED);
        }

    }
}
