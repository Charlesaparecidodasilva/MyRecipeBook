using CommomTestUtilities.Request;
using CommomTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Comunication.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Test.User.ChangePassword
{
    public class ChangePasswordInvalidTokenTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "user/change-password";

        public ChangePasswordInvalidTokenTest(CustonWebAplicationFactory webApplication) : base(webApplication)
        {
        }

        [Fact]
        public async Task Error_Token_Invalid()
        {
            var request = new RequestChangePaswordJson();

            var response = await DoPut(METHOD, request, token: "tokenInvalid");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]  
        public async Task Erro_Without_Token()
        {
            var request = new RequestChangePaswordJson();

            var response = await DoPut(METHOD, request, token: string.Empty);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public  async Task Error_Token_Whit_User_NotFound()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var request = new RequestChangePaswordJson();

            var response = await DoPut(METHOD, request, token);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }       
    }
}

