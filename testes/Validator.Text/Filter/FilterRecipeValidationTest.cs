using CommomTestUtilities.Request;
using FluentAssertions;
using MyRacipeBook.Application.UserCases.Recipe.Filter;
using MyRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator.Test.Filter
{
    public class FilterRecipeValidationTest
    {
        [Fact]

        public void Success()
        {
            var validation = new FilterRecipeValidator();

            var request = RequestFilterRecipeJsonBuilder.Build();

            var result = validation.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Invalid_Cooking_Time()
        {
            var validation = new FilterRecipeValidator();
            var request = RequestFilterRecipeJsonBuilder.Build();
            request.CookingTimes.Add((MyRecipebook.Comunication.Enum.CookingTime)1000);
            var result = validation.Validate(request);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.COOKING_TIME_NOT_SUPORTED));
        }


        [Fact]

        public void Error_invalid_Difficulty()
        {
            var validation = new FilterRecipeValidator();

            var request = RequestFilterRecipeJsonBuilder.Build();

            request.Difficulties.Add((MyRecipebook.Comunication.Enum.Difficulty)1000);

            var result = validation.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.DIFUCULT_LEVEL_NOT_SUPORT));
        }

        [Fact]

        public void Error_invalid_DhishType()
        {
            var validadtion = new FilterRecipeValidator();

            var request = RequestFilterRecipeJsonBuilder.Build();

            request.Difficulties.Add((MyRecipebook.Comunication.Enum.Difficulty)1000);

            var result = validadtion.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.DISH_TYPE_NOT_SUPORTED));

        }
    }
}
