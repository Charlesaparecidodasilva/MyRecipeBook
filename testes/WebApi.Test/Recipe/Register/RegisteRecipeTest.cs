using CommomTestUtilities.Request;
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

namespace WebApi.Test.Recipe.Register
{
    public class RegisteRecipeTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "recipe";

        private readonly Guid _userIdentifi;

        public RegisteRecipeTest(CustonWebAplicationFactory factory) : base (factory)
        {
            _userIdentifi = factory.GetUserIdentifier();
        }

        [Fact]  
        public async Task Success()
        {
            var request = RequestRecipeJsonBuilder.Build();
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifi);

            var response = await DoPost(method: METHOD, requesicao: request, token : token);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            
            await using var responseBody = await response.Content.ReadAsStreamAsync();    

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
            responseData.RootElement.GetProperty("id").GetString().Should().NotBeNullOrWhiteSpace();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public  async Task Error_Title_Empaty(string culture)
        {
            var request = RequestRecipeJsonBuilder.Build();

            request.Title = string.Empty;

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifi);

            var response = await DoPost(method: METHOD, requesicao: request, token:token, culture: culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            await using var responsiBody = await response.Content.ReadAsStreamAsync();

            var responsiData = await JsonDocument.ParseAsync(responsiBody);

            var errors = responsiData.RootElement.GetProperty("erros").EnumerateArray();

            var expectMessage = ResourceMenssageException.ResourceManager.GetString("RECIPE_TITLE_EMPTY", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectMessage));

        }
    }
}
