using CommomTestUtilities.Request;
using FluentAssertions;
using MyRacipeBook.Application.UserCases.User.Register;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validator.Text.User.Register
{
    public class RegisteUserValidatorTest
    {
        [Fact]
        public void Success()
        {

            var validator = new RegisteUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();

            var result = validator.Validate(request);

            Assert.True(result.IsValid);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Error_Name_empaty()
        {
            var validator = new RegisteUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();
            
            request.Name = string.Empty;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.NAME_EMPATY));           
        }

        [Fact]
        public void Error_Email_Empaty()
        {
            var validator = new RegisteUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();

            request.Email = string.Empty;

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.EMAIL_EMPATY));
        }

        [Fact]
        public void Error_Email_Invalid()
        {
            var validator = new RegisteUserValidator();

            var request = RequestRegisterUserJsonBuilder.Build();

            request.Email = "Joao.com";

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.EMAIL_INVALID));    
        }

        //[Theory]
        //[InlineData(0)]
        //[InlineData(1)]
        //[InlineData(2)]
        //[InlineData(3)]
        //[InlineData(4)]
        //public void Error_Email_Invalid_Theory( int passwordLength)
        //{
        //    var validator = new RegisteUserValidator();

        //    var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

        //    var result = validator.Validate(request);

        //    result.IsValid.Should().BeFalse();
        //    result.Errors.Should().ContainSingle()
        //    .And.Contain(e => e.ErrorMessage.Equals(ResourceMenssageException.EMAIL_INVALID));
        //}
    }
}
