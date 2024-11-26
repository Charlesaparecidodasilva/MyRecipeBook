using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Comunication.Responses;
using MyRecipebook.Domain.Repositories.User;
using MyRecipebook.Domain.Security.Tokens;
using MyRecipeBook.Exception.ExceptionBase;
using MyRecipeBook.Exception;
using MyRecipebook.Domain.Extensions;

namespace MyRecipeBook.API.Filter
{
    public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
    {
        private readonly IAccessTokenValidator _accessTokenValidator;
        private readonly IUserReadOnlyRepository _repository;

        public AuthenticatedUserFilter(
            IAccessTokenValidator accessTokenValidator,
            IUserReadOnlyRepository repository)
        {
            _accessTokenValidator = accessTokenValidator;
            _repository = repository;

        }

        ///Implementando o IAsyncAuthorizationFilter,  ele gera esse metodo abaixo."OnAuthorizationAsync"
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                // Extrai o token JWT da requisição HTTP.
                var token = TokenOrRequest(context);

                // Valida o token e extrai o identificador do usuário.
                var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);

                // Verifica se existe um usuário ativo com o identificador fornecido.
                var exist = await _repository.ExistActiveUserWithIdentifier(userIdentifier);

                // Se o usuário não existir ou estiver inativo, lança uma exceção personalizada.
                if (exist.IsFalse())
                {
                    throw new MyRecipeBookException(ResourceMenssageException.USER_WITHOUTE_PERMISSION_ACCES_RESOURCE);
                }
            }
            catch (SecurityTokenExpiredException)
            {
                // Se o token expirou, retorna uma resposta de não autorizado com uma mensagem específica.
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJason("TokenIsExpired")
                {
                    TokenIsExired = true,
                });
            }
            catch (MyRecipeBookException ex)
            {
                // Trata exceções específicas da aplicação, retornando uma mensagem de erro personalizada.
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJason(ex.Message));
            }
            catch
            {
                // Captura qualquer outra exceção, retornando uma mensagem de acesso negado genérica.
                context.Result = new UnauthorizedObjectResult(new ResponseErrorJason(ResourceMenssageException.USER_WITHOUTE_PERMISSION_ACCES_RESOURCE));
            }
        }

        private static string TokenOrRequest(AuthorizationFilterContext context)
        {
            // Obtém o cabeçalho de autorização da requisição HTTP.
            var authentication = context.HttpContext.Request.Headers.Authorization.ToString();

            // Verifica se o token está presente; se não, lança uma exceção personalizada.
            if (string.IsNullOrWhiteSpace(authentication))
            {
                throw new MyRecipeBookException(ResourceMenssageException.NO_TOKEN);
            }

            // Remove o prefixo "Bearer " do token e retorna apenas o token limpo.
            return authentication["Bearer ".Length..].Trim();
        }
    }
}

