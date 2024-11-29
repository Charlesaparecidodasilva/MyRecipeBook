using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyRecipebook.Comunication.Enum;
using MyRecipebook.Domain.Entities;
using MyRecipeBook.Infrastruture.DataAcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Test
{                                        // A classe WebApplicationFactory simula o comportamento completo da aplicação, desde as requisições HTTP até as interações com o banco de dados
                                         // Define uma classe customizada para criar um ambiente de teste com a infraestrutura da aplicação.
    public class CustonWebAplicationFactory : WebApplicationFactory<Program>
    {
        // Armazena uma entidade de usuário para uso nos testes.
        private MyRecipebook.Domain.Entities.User _user;
        private MyRecipebook.Domain.Entities.Recipe _recipe;
        private string _password;
        public string GetEmail() => _user.Email; 
        public string GetPassword() => _password;
        public string GetName () => _user.Name;
        public string GetRecipeId () => IdEncripterBuilder.Build().Encode(_recipe.Id);
        public Guid GetUserIdentifier() => _user.UserIdentifier;
        public string GetRecipeTitle() => _recipe.Title;
        public MyRecipebook.Domain.Enum.Difficulty GetRecipeDifficulty() => _recipe.Difficulty!.Value;
        public MyRecipebook.Domain.Enum.CookingTime GetRecipeCookingTime() => _recipe.CookingTime!.Value;     
        public IList<MyRecipebook.Domain.Enum.DishType>? GetDishType() => _recipe.DishTypes.Select(c => c.Type).ToList();
       
        // Configura o host da aplicação para os testes.
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Define o ambiente como "Test" para utilizar configurações específicas, como o appsettings.Test.json.
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    // Encontra o serviço que representa a configuração do DbContext do banco de dados real.
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MyRecipeBookDbContext>));

                    // Remove o serviço de banco de dados real se ele estiver presente.
                    if (descriptor is not null)
                    {
                        services.Remove(descriptor);
                    }
                    // Adiciona um banco de dados em memória para simular operações de banco de dados durante os testes.
                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<MyRecipeBookDbContext>(options =>
                    {
                        // Configura o DbContext para usar o banco de dados em memória.
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                        // Define o provedor de serviços interno para o DbContext.
                        options.UseInternalServiceProvider(provider);
                    });

                    // Cria um escopo de serviço para manipular o contexto do banco de dados.
                    using var scope = services.BuildServiceProvider().CreateScope();

                    // Obtém o DbContext para configurar o banco de dados inicial para os testes.
                    var dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();

                    // Garante que qualquer instância existente do banco de dados em memória seja excluída para evitar dados de testes anteriores.
                    dbContext.Database.EnsureDeleted();

                    // Inicializa o banco de dados com dados de exemplo.
                    StartDataBase(dbContext);
                });
        }

        // Método auxiliar para popular o banco de dados com dados iniciais.
        private void StartDataBase(MyRecipeBookDbContext dbContext)
        {
            // Cria um usuário e uma senha para uso nos testes.
            (_user, _password) = UserBuilder.Build();

            //cria uma receita
            _recipe = RecipeBuilder.Build(_user);

            // Adiciona o usuário ao banco de dados.
            dbContext.Users.Add(_user);

            //adiciona a receita no banco de dados.
            dbContext.Recipes.Add(_recipe);

            // Salva as alterações no banco de dados.
            dbContext.SaveChanges();
        }
    }
       

}
