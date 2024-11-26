using MyRecipebook.Domain.Security.Tokens;

namespace MyRecipeBook.API.Token
{
    /// <summary>
    /// Classe responsável por fornecer o valor do token JWT a partir do contexto HTTP.
    /// </summary>
    /// 
    ///Ele deve ser configurado em programs builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
    
    public class HttpContextTokenValue : ITokenProvider
    {
        /// E tambem deve colocar o builder.Services.AddHttpContextAccessor()  para o IHttpContextAccessor funcionar;
        // Campo somente leitura para acessar o contexto HTTP.
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Construtor que inicializa o acesso ao contexto HTTP.
        /// </summary>
        /// <param name="contextAccessor">Objeto para acessar o contexto HTTP atual.</param>
        public HttpContextTokenValue(IHttpContextAccessor contextAccessor)
        {
            // Inicializa o campo _contextAccessor com o valor passado no construtor.
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Obtém o valor do token JWT a partir dos cabeçalhos da requisição HTTP.
        /// </summary>
        /// <returns>O token JWT como uma string.</returns>
        public string Value()
        {
            // Obtém o valor do cabeçalho de autorização da requisição HTTP atual.
            var authentication = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
            

            // Remove o prefixo "Bearer " do valor do cabeçalho e retorna o token JWT.
            return authentication["Bearer ".Length..].Trim();
        }
    }
}
