using CommomTestUtilities.Request;
using MyRecipeBook.Comunication.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using MyRecipeBook.Exception;
using System.Globalization;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin
{
    public class DoLoginTest : MyRecipeBookClassFixture
    {
        private readonly string method = "login";              
        private readonly string _password;
        private readonly string _email;
        private readonly string _name;
        public DoLoginTest(CustonWebAplicationFactory factory) : base(factory)
        {
            // Vou obter os dados do usuario e o Cliente Http dp CustonWebAplicationFactory          
           _email = factory.GetEmail();
           _password = factory.GetPassword();
           _name = factory.GetName();
        }

        [Fact]  
        public async Task Success()
        {
            // vou gerar uma request com os dados gerados pelo CustonWebAplicationFactory
            var request = new RequestLoguinJason
            {
                Email = _email,
                Password = _password
            };
           
            var resposta = await DoPost( method, request);      
            
            resposta.StatusCode.Should().Be(HttpStatusCode.OK);         

            await using var corpodeResposta = await resposta.Content.ReadAsStreamAsync();

            var respostaData = await JsonDocument.ParseAsync(corpodeResposta);    
            
            respostaData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name);


        }


        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Erro_Login_Invalid(string cultura)
        {
            var requesicao = RequestLoginJasonBuilder.Build();
        
            var resposta = await DoPost(method, requesicao, cultura);

            resposta.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            await using var corpodeResposta = await resposta.Content.ReadAsStreamAsync();

            var respostaData = await JsonDocument.ParseAsync(corpodeResposta);

            var errors = respostaData.RootElement.GetProperty("errors").EnumerateArray();

            var expectMenssage = ResourceMenssageException.ResourceManager.GetString("INVALID_EMAIL_OR_PASSWORD", new CultureInfo(cultura));

            errors.Should().ContainSingle().And.Contain(errors => errors.GetString().Equals(expectMenssage));

        }

    }
}
