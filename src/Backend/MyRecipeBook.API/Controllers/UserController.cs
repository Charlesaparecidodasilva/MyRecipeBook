using FluentMigrator.Runner.Generators;
using Microsoft.AspNetCore.Mvc;
using MyRacipeBook.Application.UserCases.User.ChangePassword;
using MyRacipeBook.Application.UserCases.User.Profile;
using MyRacipeBook.Application.UserCases.User.Register;
using MyRacipeBook.Application.UserCases.User.Update;
using MyRecipeBook.API.Attributes;
using MyRecipeBook.Comunication.Request;
using MyRecipeBook.Comunication.Responses;
namespace MyRecipeBook.API.Controllers
{


    public class UserController : MyRecipeBookBaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJason), StatusCodes.Status201Created)]

        public async Task<IActionResult> Register(
            [FromServices] IRegisterUserUseCase useCase,
            [FromBody] RequestRegisterUserJson request)
        {

            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
        }
    
        [HttpGet]
        [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
        [AuthenticatedUser]
        public async Task<IActionResult> GetUserProfile(
            [FromServices] IGetUserProfileUseCase userCase)
        {
            var result = await userCase.Execute();

            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJason), StatusCodes.Status400BadRequest)]
        [AuthenticatedUser]

        public async Task<IActionResult>Update(
            [FromServices]IUpdateUserUseCase userCase,
            [FromBody] RequestUpdateUserJson request
            )
        {

            await userCase.Execute(request);
            return NoContent();
        }


        [HttpPut("change-password")]

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJason), StatusCodes.Status400BadRequest)]
        [AuthenticatedUser]

        public async Task<IActionResult> ChangePassWord(
            [FromServices] IChangePasswordUserCase useCase,
            [FromBody] RequestChangePaswordJson rquest)
            
        {
            await useCase.Execult(rquest);

            return NoContent();

        }
    }
}
