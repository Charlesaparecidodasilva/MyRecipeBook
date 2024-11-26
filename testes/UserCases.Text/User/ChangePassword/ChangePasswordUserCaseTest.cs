using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggerUser;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Request;
using FluentAssertions;
using MyRacipeBook.Application.UserCases.User.ChangePassword;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using MyRecipeBook.Infrastruture.DataAcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserCases.Test.User.ChangePassword
{
    public class ChangePasswordUserCaseTest
    {

        [Fact]  
        public async Task Success()
        {
            (var user, var password) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();

            request.Password = password;

            var userCase = CreateUserCase(user);

            Func<Task> act = async () => await userCase.Execult(request);

            await act.Should().NotThrowAsync();

            var passwordEncripter = PasswordEncripterBuilder.Build();

            user.Password.Should().Be(passwordEncripter.Encrypt(request.NewPassword));
        }

        [Fact]
        public async Task Error_NewPassword_Empty()
        {
            // Cria um usuário e senha usando o UserBuilder
            (var user, var password) = UserBuilder.Build();

            // Cria a requisição de alteração de senha com uma nova senha vazia
            var request = new RequestChangePaswordJson
            {
                Password = password,
                NewPassword = string.Empty
                
            };

            // Cria o caso de uso com o usuário construído
            var useCase = CreateUserCase(user);

            // Define uma função que executa o caso de uso, para ser validada quanto a exceções
            Func<Task> act = async () => { await useCase.Execult(request); };

            // Verifica se a execução lança uma exceção do tipo ErroOnValidationException,
            // contendo a mensagem de erro esperada
            (await act.Should().ThrowAsync<ErroOnValidationException>())
                .Where(e => e.MenssageErroValidada.Count == 1 &&
                            e.MenssageErroValidada.Contains(ResourceMenssageException.PASSWORD_EMPATY));

            // Verifica se a senha do usuário permanece igual, usando o PasswordEncripter
            var passwordEncrypter = PasswordEncripterBuilder.Build();
            user.Password.Should().Be(passwordEncrypter.Encrypt(password));
        }

        [Fact]
        public async Task Erro_Passwor_Diferent()
        {

            (var user, var password) = UserBuilder.Build();

            var request = RequestChangePasswordJsonBuilder.Build();

            var useCase = CreateUserCase(user);

            Func<Task> act = async () => { await useCase.Execult(request); };

            await act.Should().ThrowAsync<ErroOnValidationException>()
                .Where(e => e.MenssageErroValidada.Count == 1 &&
                e.MenssageErroValidada.Contains(ResourceMenssageException.PASSWOR_IS_DIFERENT));

            var passwordEncripter = PasswordEncripterBuilder.Build();

            user.Password.Should().Be(passwordEncripter.Encrypt(password));

        }


        private static ChangePasswordUserCase CreateUserCase(MyRecipebook.Domain.Entities.User user)
        {                   
            var loggerUser = LoggerUserBuilder.Build(user);
            var passwordEncriptor = PasswordEncripterBuilder.Build();
            var unitOfWork = UnitiOfWorkBuilder.Build();
            var userUpdateRepository = new UserUpdateOnlyRepositoryBuild().GetById(user).Build();

            return new ChangePasswordUserCase(loggerUser, passwordEncriptor, userUpdateRepository,unitOfWork ); 
        }



    }
}
