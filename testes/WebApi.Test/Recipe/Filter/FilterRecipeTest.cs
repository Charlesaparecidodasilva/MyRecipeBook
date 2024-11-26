using CommomTestUtilities.Request;
using CommomTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Comunication.Request;
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

namespace WebApi.Test.Recipe.Filter
{
    public class FilterRecipeTest : MyRecipeBookClassFixture
    {
        private const string METHOD = "recipe/filter";

        private readonly Guid _userIdentifier;

        private string _recipeTitle;

        private MyRecipebook.Domain.Enum.Difficulty _recipedifficultyLevel;
        private MyRecipebook.Domain.Enum.CookingTime _recipeCookingTime;
        private IList<MyRecipebook.Domain.Enum.DishType> _recipeDishTypes;

        public FilterRecipeTest(CustonWebAplicationFactory factory) : base(factory)
        {
            _userIdentifier = Guid.NewGuid();
            _recipeTitle = factory.GetRecipeTitle();
             _recipeCookingTime = factory.GetRecipeCookingTime();
            _recipedifficultyLevel = factory.GetRecipeDifficulty();
            _recipeDishTypes = factory.GetDishType();
        }
        // Atributo [Fact] indica que este é um teste unitário usando o framework xUnit.
        [Fact]
        public async Task Success()
        {
            // Cria uma instância da classe RequestFilterRecipeJson e inicializa suas propriedades.
            var request = new RequestFilterRecipeJson
            {
                // Define a lista CookingTimes com um valor baseado na variável _recipeCookingTime convertida para o enum CookingTime.
                CookingTimes = [(MyRecipebook.Comunication.Enum.CookingTime)_recipeCookingTime],

                // Define a lista Difficulties com um valor baseado na variável _recipedifficultyLevel convertida para o enum Difficulty.
                Difficulties = [(MyRecipebook.Comunication.Enum.Difficulty)_recipedifficultyLevel],

                // Converte cada elemento da lista _recipeDishTypes para o enum DishType e cria uma lista.
                DishTypes = _recipeDishTypes.Select(dishTypes => (MyRecipebook.Comunication.Enum.DishType)dishTypes).ToList(),

                // Atribui à propriedade RecipeTitle_Ingredientes o valor da variável _recipeTitle.
                RecipeTitle_Ingredientes = _recipeTitle,
            };

            // Gera um token JWT (JSON Web Token) usando o identificador do usuário (_userIdentifier).
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            // Realiza uma requisição HTTP POST usando o método DoPost.
            // Parâmetros:
            //   - method: O endpoint da API representado pela constante METHOD.
            //   - requesicao: O objeto request criado acima.
            //   - token: O token JWT gerado para autenticação.
            var response = await DoPost(method: METHOD, requesicao: request, token: token);

            // Verifica se o status da resposta HTTP é 200 OK, o que indica sucesso na requisição.
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Lê o conteúdo da resposta como um stream de forma assíncrona.
            await using var responsyBody = await response.Content.ReadAsStreamAsync();

            // Faz o parsing do stream da resposta para um objeto JSON usando JsonDocument.
            var responseData = await JsonDocument.ParseAsync(responsyBody);
        }

        [Fact]
        public async Task Success_NoContent()
        {
            var request = RequestFilterRecipeJsonBuilder.Build();

            request.RecipeTitle_Ingredientes = "recipeDontExist";

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPost(method: METHOD, requesicao: request, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_CookingTime_Invalid(string culture)
        {
            var request = RequestFilterRecipeJsonBuilder.Build();

            //atribui um valor falso ao cookingTime
            request.CookingTimes.Add((MyRecipebook.Comunication.Enum.CookingTime)1000);

            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoPost(method: METHOD, requesicao: request, token: token);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("erros").EnumerateArray();

            var expectedMessage = ResourceMenssageException.ResourceManager.GetString("COOKING_TIME_NOT_SUPORTED", new CultureInfo(culture));

            errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));


        }

    }
}
