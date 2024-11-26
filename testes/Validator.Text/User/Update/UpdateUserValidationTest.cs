using CommomTestUtilities.Entities;
using CommomTestUtilities.Request;
using FluentAssertions;
using MyRacipeBook.Application.UserCases.User.Update;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator.Test.User.Update
{
    public class UpdateUserValidationTest
    {

        [Fact]

        public void Sucess()
        {
            var validation = new UpdateUserValidation();

            var request = RequestUpdateUserJsonBuilder.Build();

            var result = validation.Validate(request);

            result.IsValid.Should().BeTrue();

        }

        [Fact]

        public void Error_Name_Empaty()
        {
          
            var validation = new UpdateUserValidation();
            var request = RequestUpdateUserJsonBuilder.Build();

            request.Name = string.Empty;

            var result = validation.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().
                And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.NAME_EMPATY));
        }

        [Fact]
        public void Error_Email_Empaty()
        {
            var validation = new UpdateUserValidation();
            var request = RequestUpdateUserJsonBuilder.Build();

            request.Email = string.Empty;

            var result = validation.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().
                And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.EMAIL_EMPATY));
        }

        [Fact]

        public void Error_Email_Invalid()
        {

            var validation = new UpdateUserValidation();
            var request = RequestUpdateUserJsonBuilder.Build();

            request.Email = "email";

            var result = validation.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().
                And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.EMAIL_INVALID));

        }


    }
}
