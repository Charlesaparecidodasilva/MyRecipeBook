using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using MyRacipeBook.Application.UserCases.Recipe;
using MyRacipeBook.Application.UserCases.Recipe.Delete;
using MyRacipeBook.Application.UserCases.Recipe.Filter;
using MyRacipeBook.Application.UserCases.Recipe.GetById;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.API.Binders;
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

        [ProducesResponseType(typeof(ResponseRecipesJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<IActionResult> Filter(
            [FromServices] IFilterRecipeUseCase useCase,
            [FromBody] RequestFilterRecipeJson request
            )
        {
            var resposta = await useCase.Execulte(request);

            if (resposta.Recipes.Any())
                return Ok(resposta);
                
            return NoContent();
        }
     
        ///
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseRecipeJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJason),StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetById(
             [FromServices] IGetRecipeByIdUserCase useCase,
             [FromRoute] [ModelBinder(typeof(MyRecipeBookIdBinder))] long id )
        {
            var response = await useCase.Execute(id);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJason), StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Delete(
            [FromServices] IDeleteRecipeUserCase userCase,
            [FromRoute][ModelBinder(typeof(MyRecipeBookIdBinder))] long id)
        {
            await userCase.Execute(id);

            return NoContent();

        }


    }
}
