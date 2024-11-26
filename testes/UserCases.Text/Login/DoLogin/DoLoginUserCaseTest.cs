using Castle.Core.Resource;
using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Entities;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Request;
using CommomTestUtilities.Tokens;
using FluentAssertions;
using FluentAssertions.Execution;
using MyRacipeBook.Application.UserCases.Login.DoLogin;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Comunication.Responses;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
namespace UserCases.Test.Login.DoLogin
{
    public class DoLoginUserCaseTest
    {
       
        
        
        [Fact]
        public async Task Sucess()
        {
            (var user, var password) =  UserBuilder.Build();

            (var use, var passwo) = UserBuilder.Build();

            var useCase = CriarUseCase(use);

            var userCase = CriarUseCase(user);

            var result = await userCase.Execute( new RequestLoguinJason
            {
                Email = user.Email,
                Password = password

            });

            result.Should().NotBeNull();
            result.Tokens.Should().NotBeNull();
            result.Name.Should().NotBeNullOrWhiteSpace().And.Be(user.Name);
            result.Tokens.AccessToken.Should().NotBeNullOrEmpty();

        }

        [Fact]
        public async Task Erro_Invalid_User()
        {                             
            //geradadosFalsos
            var requesicaoFalsa = RequestLoginJasonBuilder.Build();

            var userCase = CriarUseCase();

            Func<Task> act = async () => { await userCase.Execute(requesicaoFalsa);};
            
            await act.Should().ThrowAsync<InvalidLoginException>()
                 .Where(e => e.Message.Equals(ResourceMenssageException.INVALID_EMAIL_OR_PASSWORD));
        }


        //CriarUseCase passa a representar DoLoginUserCase
        private static DoLoginUserCase CriarUseCase(MyRecipebook.Domain.Entities.User? user= null )
        {
            //instancia da classe PasswordEncripterBuilder
            var passwordEncripter = PasswordEncripterBuilder.Build();
            // Com isso eele simula uma consulta no banco de dados.
            var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
            var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

            if (user is not null)   //Se o user foi informado, o método GetByEmailAndPassword(user) é chamado,
                                    //simulando o armazenamento dos dados do usuário no repositório de leitura
                                    //para que possam ser acessados no fluxo de autenticação.
                userReadOnlyRepositoryBuilder.GetByEmailAndPassword(user);


            // Chamar o Build sem chamar a funcao GetByEmailAndPassword apenas retorna o valor boleano false por padrao.
            // Chamar o Build depois de executar a funcao GetByEmailAndPassword retorna od dados retornados pela funão armaazenado nos repository.
            return new DoLoginUserCase(userReadOnlyRepositoryBuilder.Build(), accessTokenGenerator, passwordEncripter);
        }
    }
}

