using Azure;
using CommomTestUtilities.Request;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.SqlServer.Server;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MyRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register
{                                                  // A classe que eu criei denro da api, 
    public class RegisterUserTest : MyRecipeBookClassFixture
    {
        private readonly string method = "user";

       
        public RegisterUserTest(CustonWebAplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Success()
        {
            // Cria uma requisição JSON de registro de usuário usando o builder.
            var requisicao = RequestRegisterUserJsonBuilder.Build();

            // Envia uma requisição HTTP POST para o endpoint "User" da API,
            // com os dados criados anteriormente na requisição em formato JSON.
            var resposta = await DoPost(method, requisicao);

            // Verifica se a resposta da API tem o código de status Created (201), indicando sucesso na criação.
            resposta.StatusCode.Should().Be(HttpStatusCode.Created);

            // Lê o conteúdo da resposta como um stream para análise.
            await using var corpoDaResposta = await resposta.Content.ReadAsStreamAsync();

            // Converte os dados da resposta para um documento JSON.
            var respostaData = await JsonDocument.ParseAsync(corpoDaResposta);

            // Valida se a propriedade "name" do JSON retornado é igual ao nome enviado na requisição.
            respostaData.RootElement.GetProperty("name").GetString()
                .Should().NotBeNullOrWhiteSpace().And.Be(requisicao.Name);

            // Verifica se o "accessToken" está presente e não está vazio.
            respostaData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString()
                .Should().NotBeNullOrEmpty();
        }

        [Theory]    
        [ClassData(typeof(CultureInlineDataTest))]     
        public async Task Erro_Empaty_Name(string cultura)
        {
            var requesicao = RequestRegisterUserJsonBuilder.Build();

            requesicao.Name = string.Empty;

            //Verificando se ja existe um Accept-Language, caso tenha, remova o
          

            var resposta = await DoPost( method: method, requesicao : requesicao, culture: cultura);
            
            resposta.StatusCode.Should().Be(HttpStatusCode.BadRequest);
           
            await using var corpodeResposta = await resposta.Content.ReadAsStreamAsync();
           
            var respostaData = await JsonDocument.ParseAsync(corpodeResposta);

            var errors = respostaData.RootElement.GetProperty("errors").EnumerateArray();

            var expectMenssage = ResourceMenssageException.ResourceManager.GetString("NAME_EMPATY", new CultureInfo(cultura));

            errors.Should().ContainSingle().And.Contain(errors => errors.GetString().Equals(expectMenssage));
        }
    }
}
