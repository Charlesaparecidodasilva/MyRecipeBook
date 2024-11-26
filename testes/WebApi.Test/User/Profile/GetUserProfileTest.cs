using CommomTestUtilities.Tokens;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Test.User.Profile
{
    public class GetUserProfileTest : MyRecipeBookClassFixture
    {
        private readonly string METHOD = "user";

        private readonly string _name;

        private readonly string _email;

        private readonly Guid _userIdentifier;

        public GetUserProfileTest(CustonWebAplicationFactory factory) : base(factory)
        {

            _name = factory.GetName();
            _email = factory.GetEmail();
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            // Gera um token JWT usando o método 'Build' do 'JwtTokenGeneratorBuilder' e o 'Generate' passando o identificador do usuário
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            // Realiza uma requisição HTTP GET ao método especificado em 'METHOD', passando o token gerado para autenticação
            var response = await DoGet(METHOD, token: token);

            // Verifica se o status da resposta HTTP é 'OK' (código 200), indicando sucesso na requisição
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Lê o corpo da resposta HTTP como um stream assíncrono e atribui ao 'responseBody'
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            // Analisa o conteúdo JSON da resposta e o armazena em 'responseData' para facilitar o acesso aos dados
            var responseData = await JsonDocument.ParseAsync(responseBody);

            // Extrai o valor da propriedade 'name' do JSON, verifica se não é nulo ou vazio e se corresponde ao valor esperado em '_name'
            responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name);

            // Extrai o valor da propriedade 'email' do JSON, verifica se não é nulo ou vazio e se corresponde ao valor esperado em '_email'
            responseData.RootElement.GetProperty("email").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_email);
        }

    }

}
