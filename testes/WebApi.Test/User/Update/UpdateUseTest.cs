using CommomTestUtilities.Request;
using CommomTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Exception;
using MyRecipeBook.Infrastruture.Security.Tokens.Acess.Generator;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update
{
    public class UpdateUseTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "user";

        private readonly Guid _userIdentifier;

        public UpdateUseTest(CustonWebAplicationFactory factory) : base (factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Succes()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
            var response = await DoPut(METHOD, request, token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }


        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empaty_Name(string culture)
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty;
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPut(METHOD, request, token, culture);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await  JsonDocument.ParseAsync(responseBody);

            var error = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMenssageException.ResourceManager.GetString("NAME_EMPATY", new CultureInfo(culture));

            error.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));

        }
             
    }
}
