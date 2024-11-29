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

namespace WebApi.Test.Recipe.GetById
{
    public class GetRecipeByIdTest : MyRecipeBookClassFixture
    {

        private const string METHOD = "recipe";

        private readonly Guid _userIdentifi;
        private readonly string _recipeId;
        private readonly string _recipeTitle;

        public GetRecipeByIdTest(CustonWebAplicationFactory factory) : base (factory)
        {
            _userIdentifi = factory.GetUserIdentifier();
            _recipeId = factory.GetRecipeId();
            _recipeTitle = factory.GetRecipeTitle();
        }


        [Fact]
        public async Task  Sucess()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifi);

            var response = await DoGet($"{METHOD}/{_recipeId}", token);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            await using var resposeBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(resposeBody);

            responseData.RootElement.GetProperty("id").GetString().Should().Be(_recipeId);
            responseData.RootElement.GetProperty("title").GetString().Should().Be(_recipeTitle);
           
        }


        [Theory] /// Não encontrada por que o id que estou passando nao existe
        [ClassData(typeof(CultureInlineDataTest))] 
        public async  Task  Error_Recipe_Not_Found(string culture)
        {
            
                var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifi);

                var id = IdEncripterBuilder.Build().Encode(1000);

                var response = await DoGet($"{METHOD}/{id}", token, culture);

                response.StatusCode.Should().Be(HttpStatusCode.NotFound);

                await using var responsBody = await response.Content.ReadAsStreamAsync();

                var responseData = await JsonDocument.ParseAsync(responsBody);

                var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

                var expectedMessage = ResourceMenssageException.ResourceManager.GetString("RECIPE_NOT_FOUND", new CultureInfo(culture));

                errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));             
        }
    }
}