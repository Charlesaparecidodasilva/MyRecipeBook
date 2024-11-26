using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Request;
using CommomTestUtilities.Tokens;
using FluentAssertions;
using MyRacipeBook.Application.UserCases.User.Register;
using MyRecipebook.Domain.Security.Tokens;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using MyRecipeBook.Infrastruture.Security.Tokens.Acess.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCases.Text.User.Register
{
    public class RegisterUserUseTest
    {
        [Fact]
        public async Task Success()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUserCase();       
                            //Esse é o execulte real
            var result =  await useCase.Execute(request);

            result.Should().NotBeNull();
            result.Tokens.Should().NotBeNull();
            result.Name.Should().Be(request.Name);       
            result.Tokens.AccessToken.Should().NotBeNullOrEmpty();

        }

        [Fact]
        private async Task Error_Email_Already_Registered()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            var useCase = CreateUserCase(request.Email);
                                           //Esse é o execulte real
            Func<Task> act = async () => await useCase.Execute(request);

            (await act.Should().ThrowAsync<ErroOnValidationException>())
                .Where(e => e.MenssageErroValidada.Count == 1 && e.MenssageErroValidada.Contains(ResourceMenssageException.EMAIL_ALREADY_REGISTERED));         
        }

        [Fact]
        private async Task Erro_Nome_Vazio()
        {
            var request = RequestRegisterUserJsonBuilder.Build();

            request.Name = string.Empty;
            
            var useCase = CreateUserCase();
                                         //Esse é o execulte real
            Func<Task> act = async () => await useCase.Execute(request);

            (await act.Should().ThrowAsync<ErroOnValidationException>())
                .Where(e => e.MenssageErroValidada.Count == 1 && e.MenssageErroValidada.Contains(ResourceMenssageException.NAME_EMPATY));
        }



        private RegisterUserUseCase CreateUserCase(string? email = null)
        {
            var mapper = MapperBuilder.Build();
            var senhaCript = PasswordEncripterBuilder.Build();
            var userWriteOnly = UserWriteOnlyRepositoryBuilder.Build();
            var unitWork = UnitiOfWorkBuilder.Build();
            var readRepositoryBuild = new UserReadOnlyRepositoryBuilder();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

            if (string.IsNullOrEmpty(email) == false)
                   
                readRepositoryBuild.ExistActiveUserWhitEmail(email);

            return new RegisterUserUseCase(userWriteOnly, readRepositoryBuild.Build(), unitWork, senhaCript, accessTokenGenerator, mapper);

        } 
    }
}



