using Microsoft.EntityFrameworkCore;
using MyRecipebook.Domain.Entities;
using MyRecipebook.Domain.Security.Tokens;
using MyRecipebook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastruture.DataAcess;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastruture.Services.LoggedUser
{
    /// <summary>
    /// Classe responsável por recuperar informações do usuário logado com base em um token JWT.
    /// </summary>
    public class LoggedUser : ILoggedUser
    {
        // Campo somente leitura para o contexto do banco de dados.
        private readonly MyRecipeBookDbContext _dbContext;

        // Campo somente leitura para o provedor de token.
        private readonly ITokenProvider _tokenProvider;

        /// <summary>
        /// Construtor que inicializa o contexto do banco de dados e o provedor de token.
        /// </summary>
        /// <param name="dbContext">O contexto do banco de dados para interagir com os dados do usuário.</param>
        /// <param name="tokenProvider">O provedor de token para obter o token JWT do usuário.</param>
        public LoggedUser(MyRecipeBookDbContext dbContext, ITokenProvider tokenProvider)
        {
            // Inicializa o campo _dbContext com o valor passado no construtor.
            _dbContext = dbContext;

            // Inicializa o campo _tokenProvider com o valor passado no construtor.
            _tokenProvider = tokenProvider;
        }

        /// <summary>
        /// Método assíncrono que retorna o usuário logado com base no token JWT.
        /// </summary>
        /// <returns>Um objeto User representando o usuário logado.</returns>
        public async Task<User> User()
        {
            // Obtém o token JWT do usuário logado usando o provedor de token.
            var token = _tokenProvider.Value();

            // Cria uma nova instância do manipulador de tokens JWT.
            var tokenHandler = new JwtSecurityTokenHandler();

            // Lê e decodifica o token JWT para extrair as informações contidas nele.
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            // Extrai o identificador do usuário (SID) do token JWT.
            var indentifi = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

            var Useridentifi = Guid.Parse(indentifi);

            // Consulta a tabela Users no banco de dados para encontrar o usuário com o identificador extraído do token.
            // A consulta é feita sem rastrear as alterações na entidade para melhorar o desempenho.
            return await _dbContext
                .Users
                .AsNoTracking()
                .FirstAsync(user => user.Active && user.UserIdentifier == Useridentifi);
        }
    }
}
