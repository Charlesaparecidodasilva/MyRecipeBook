using CommomTestUtilities.Tokens;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Test.User.Profile
{
    public class GetUserProfileInvalidTokenTest : MyRecipeBookClassFixture
    {
        private readonly string METHOD = "user";

        public GetUserProfileInvalidTokenTest(CustonWebAplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Error_Token_Invalid()
        {
            // Com token invalido desconhecido.
            var response = await DoGet(METHOD, token: "tokenInvalid");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Whithou_Token()
        {
            // Com token vazio
            var response = await DoGet(METHOD, token: string.Empty);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        }

        [Fact]
        public async Task Error_Token_Whith_User_NotFound()
        {
            //O token vai ser válido, mas nao vai existir nenhum usuario com esse id.

            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoGet(METHOD, token);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }
    }
}

