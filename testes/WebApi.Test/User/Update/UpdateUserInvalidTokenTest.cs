using CommomTestUtilities.Request;
using CommomTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Comunication.Request;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Test.User.Update
{
    public class UpdateUserInvalidTokenTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "user";

        public UpdateUserInvalidTokenTest(CustonWebAplicationFactory webApplication) : base(webApplication)
        {

        }

        [Fact]
        public async Task Error_Token_Invalid()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            var response = await DoPut(METHOD, request, token: "TokenInvalid");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Without_Token()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            var response = await DoPut(METHOD, request, token: string.Empty);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }

        [Fact]  
        public async Task Erro_Token_With_User_NotFound()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoPut(METHOD, request, token);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }


}
