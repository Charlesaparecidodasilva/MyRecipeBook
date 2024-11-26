using CommomTestUtilities.Request;
using FluentAssertions;
using MyRacipeBook.Application.UserCases.User.ChangePassword;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator.Test.User.ChangPassword
{
    public class ChangPasswordValidationTest
    {
        [Fact]

        public void Succes()
        {
            var validator  = new ChangePasswordValidator();
           
            var request = RequestChangePasswordJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        [Theory]        
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]      
        public void Error_Password_Invalid( int passwordLength)
        {

            var validator = new ChangePasswordValidator();

            var request = RequestChangePasswordJsonBuilder.Build(passwordLength);
           
            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.INVALID_EMAIL_OR_PASSWORD));
        }

        [Fact]
        public void Error_Password_Empty()
        {

            var validator = new ChangePasswordValidator();

            var request = RequestChangePasswordJsonBuilder.Build();

            request.NewPassword = string.Empty;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();

            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.PASSWORD_EMPATY));
        }
    }
}
