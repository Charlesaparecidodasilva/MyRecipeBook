using Microsoft.AspNetCore.Mvc;
using MyRacipeBook.Application.UserCases.Login.DoLogin;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Comunication.Responses;

namespace MyRecipeBook.API.Controllers
{
    public class LoginController : MyRecipeBookBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJason), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJason), StatusCodes.Status401Unauthorized)]
       
        public async Task<IActionResult> Login(
            [FromServices] IDoLoginUserCase userCase,
            [FromBody] RequestLoguinJason request)
        {
            var response = await userCase.Execute(request);

            return Ok(response);
        }
    }
}

