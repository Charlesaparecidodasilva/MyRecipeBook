using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Tokens;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Test.Recipe.GetById
{
    public class GetRecipeByIdInvalidTokenTest : MyRecipeBookClassFixture
    {

        private const string METHOD = "recipe";

        public GetRecipeByIdInvalidTokenTest(CustonWebAplicationFactory webAplicationFactory) : base(webAplicationFactory)
        {

        }

        [Fact]
        public async Task Error_Token_Invalid()
        {
            var id = IdEncripterBuilder.Build().Encode(1);

            var response = await DoGet($"{METHOD}/{id}", token: "tokenInvalid");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Whithout_Token()
        {
            var id = IdEncripterBuilder.Build().Encode(1);

            var response = await DoGet($"{METHOD}/{id}", token: string.Empty);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }


        [Fact]
        public async Task Error_Token_Whit_User_NotFound()
        {
            var id = IdEncripterBuilder.Build().Encode(1);

            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoGet($"{METHOD}/{id}", token: token);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }




    }
}
