using CommomTestUtilities.Request;
using CommomTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.ChangePassword
{
    public class ChangePasswordTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "user/change-password";


        private readonly string _password;
        private readonly string _email;
        private readonly Guid _userIdentifier;

        public ChangePasswordTest(CustonWebAplicationFactory factory) : base(factory)
        {
            _password = factory.GetPassword();
            _email = factory.GetEmail();    
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            // Cria a requisição de mudança de senha usando o builder e define a senha atual
            var request = RequestChangePasswordJsonBuilder.Build();
            request.Password = _password;

            // Gera o token JWT usando o identificador do usuário
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            // Realiza a requisição HTTP PUT para o método especificado com a requisição de alteração de senha e o token de autenticação
            var response = await DoPut(METHOD, request, token);

            // Verifica se o status de resposta é NoContent (204), indicando sucesso na mudança de senha
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Cria uma requisição de login com o e-mail do usuário e a senha antiga
            var loginRequest = new RequestLoguinJason
            {
                Email = _email,
                Password = _password
            };

            // Realiza uma requisição HTTP POST para o endpoint de login usando a senha antiga
            response = await DoPost("login", loginRequest);

            // Verifica se o status de resposta é Unauthorized (401), indicando que a senha antiga não é mais válida
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            // Atualiza a requisição de login com a nova senha
            loginRequest.Password = request.NewPassword;

            // Realiza outra requisição HTTP POST para o endpoint de login usando a nova senha
            response = await DoPost("login", loginRequest);

            // Verifica se o status de resposta é OK (200), indicando que o login com a nova senha foi bem-sucedido
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_NewPassword_Empty(string culture)
        {
            // Cria a requisição para alteração de senha com uma nova senha vazia.
            var request = new RequestChangePaswordJson
            {
                Password = _password,
                NewPassword = string.Empty
            };

            // Gera um token JWT para autenticação.
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            // Realiza a requisição HTTP PUT ao endpoint especificado em METHOD, passando o request, o token e a cultura.
            var response = await DoPut(METHOD, request, token, culture);

            // Verifica se o código de status retornado é 400 (BadRequest), indicando erro na requisição.
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Lê o corpo da resposta como um fluxo de dados.
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            // Analisa o conteúdo JSON da resposta.
            var responseData = await JsonDocument.ParseAsync(responseBody);

            // Obtém a propriedade "errors" e itera sobre os erros.
            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            // Recupera a mensagem de erro 

        }
        }
    }
