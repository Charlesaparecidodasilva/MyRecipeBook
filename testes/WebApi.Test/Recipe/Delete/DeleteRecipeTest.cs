using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Exception;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Delete
{
    public class DeleteRecipeTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "recipe";

        private readonly Guid _userIdentifi;

        private readonly string _recipeId;

        public DeleteRecipeTest(CustonWebAplicationFactory factory) : base(factory)
        {
            _userIdentifi = factory.GetUserIdentifier();
            _recipeId = factory.GetRecipeId();
        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifi);

            var response = await DoDelete($"{METHOD}/{_recipeId}", token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            response = await DoGet($"{METHOD}/{_recipeId}", token);
       
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Recipe_Not_Found( string culture)
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifi);

            var id = IdEncripterBuilder.Build().Equals(1000);

            var response = await DoDelete($"{METHOD}/{id}", token);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var erros = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMenssageException.ResourceManager.GetString("RECIPE_NOT_FOUND", new CultureInfo(culture));

            erros.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
        }



    }
}

