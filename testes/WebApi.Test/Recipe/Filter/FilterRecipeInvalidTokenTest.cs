using CommomTestUtilities.Request;
using CommomTestUtilities.Tokens;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Test.Recipe.Filter
{
    public class FilterRecipeInvalidTokenTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "recipe/filter";

        public FilterRecipeInvalidTokenTest(CustonWebAplicationFactory webAplication) : base(webAplication)
        {

        }

        [Fact]
        public async Task Error_Token_Invalid()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();

            var response = await DoPost(method: METHOD, requesicao: request, token: "TokenInvalid");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        [Fact]
        public async Task Error_Without_Token()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();

            var response = await DoPost(method: METHOD, requesicao: request, token: string.Empty);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }
        [Fact]
        public async Task Error_Token_Whith_User_NotFound()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();
            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoPost(method: METHOD, requesicao: request, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
