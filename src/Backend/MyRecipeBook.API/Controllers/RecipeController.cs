using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using MyRacipeBook.Application.UserCases.Recipe;
using MyRacipeBook.Application.UserCases.Recipe.Filter;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Comunication.Responses;

namespace MyRecipeBook.API.Controllers
{
    [AuthenticatedUser]
    public class RecipeController : MyRecipeBookBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredRecipeJason), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJason), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisteRecipeUseCase useCase,
            [FromBody] RequestRecipeJson request)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }




        [HttpPost("filter")]

        [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        
        public async Task<IActionResult> Filter(
            [FromServices] IFilterRecipeUseCase useCase,
            [FromBody] RequestFilterRecipeJson request            
            )
        {
            var resposta =  await useCase.Execulte(request);

            if (resposta.Recipes.Any())
                return Ok(resposta);

            return NoContent();
        }      
    }
}
