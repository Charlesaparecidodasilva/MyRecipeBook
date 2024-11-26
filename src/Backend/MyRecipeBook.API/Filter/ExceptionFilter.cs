using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Comunication.Responses;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionBase;
using System.Diagnostics.Metrics;
using System.Net;

namespace MyRecipeBook.API.Filter
{
    public class ExceptionFilter : IExceptionFilter
    {
        public  void OnException(ExceptionContext contex)
        {
            //Aqui estamos verificando se o erro passado como parametro é um  MyRecipeBookException. 
            if (contex.Exception is MyRecipeBookException)
            HandleProjectExceptio(contex);
            else
            {
                ThrowUnknowException(contex);
            }
        }


        //contex é o erro que sera passado como parametro
        private static void HandleProjectExceptio(ExceptionContext contex)
        {
            if (contex.Exception is InvalidLoginException)
            {               
                contex.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                contex.Result = new UnauthorizedObjectResult(new ResponseErrorJason(contex.Exception.Message));
            }

            else if (contex.Exception is ErroOnValidationException)
            {
                //A variavel exception é uma instancia da classe ErroOnValidationException;
                var exception = contex.Exception as ErroOnValidationException;

                // context vai receber a mensagem de erro 400
                contex.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                // Aqui estou inserindo a menssagem de erro Validade da classe ErroOnValidationException que foi instanciada acima.
                contex.Result = new BadRequestObjectResult( new ResponseErrorJason(exception!.MenssageErroValidada));
            }          
        }

        private static void ThrowUnknowException(ExceptionContext contex)
        {
                                          // uso o InternalServerError para gerar a menssagem de erro 500
            contex.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                         //Agora vai ser ObjectResult
            contex.Result = new ObjectResult(new ResponseErrorJason(ResourceMenssageException.UNKNOWN_ERROR));
           
        }
    }
}

