using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyRecipebook.Domain.Enum;
using MyRecipebook.Domain.Repositories;
using MyRecipebook.Domain.Repositories.Recipe;
using MyRecipebook.Domain.Repositories.User;
using MyRecipebook.Domain.Security.Cryptography;
using MyRecipebook.Domain.Security.Tokens;
using MyRecipebook.Domain.Services.LoggedUser;
using MyRecipeBook.Infrastruture.DataAcess;
using MyRecipeBook.Infrastruture.DataAcess.Repositories;
using MyRecipeBook.Infrastruture.Extensions;
using MyRecipeBook.Infrastruture.Security.CryotoGraphy;
using MyRecipeBook.Infrastruture.Security.Tokens.Acess.Generator;
using MyRecipeBook.Infrastruture.Security.Tokens.Acess.Validator;
using MyRecipeBook.Infrastruture.Services.LoggedUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastruture
{
    public static class DependencyInjectionExtencion
    {
        public static void AddInfrastruture(this IServiceCollection services, IConfiguration configuration)
        {
            AddPasswordEncripter(services, configuration);
            AddRepositories(services, configuration);
            AddLoggedUser(services);
            AddTokens(services, configuration);
            
            // Aqui eu retorno o valor boleano que confirma se o appseting que estou usando é um Test
            if (configuration.IsUnitTestEnviroment())
                return;

            var databaseType = configuration.DataBaseType();
          
            if (databaseType == DatabaseType.MySql)
            {
                AddContext_MySqlServer(services, configuration);
                AddFluentMigration_MySql(services, configuration);
            }              
            else
            {
                AddContext_SqlServer(services, configuration);
                AddFluentMigration_SqlServer(services, configuration);
            }            
           
        }

        //conexão com o banco de dados.
        public static void AddContext_SqlServer(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionSQLServer");

            services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions =>
            {
                // Para funcionar tem que instalar o Entity.FrameworkCore.SqlServer
                dbContextOptions.UseSqlServer(connectionString);
            });
        }
    
        public static void AddContext_MySqlServer(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionMySQLServer");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 38));

            services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions =>
            {
                // Para funcionar tem que instalar o Pomelo.EntityFrameworkCore.MySql
                dbContextOptions.UseMySql(connectionString, serverVersion);
            });
        }
    
        public static void AddRepositories(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitiOfWork, UnitiOfWork>();
            //Aqui verifico se algum usuario do _dbContext(banco de dados) tem o email igual ao email que sera passado, e se ele é ativo
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();           
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
            services.AddScoped<IRecipeWriteOnlyRespository, RecipeRepository>();
            services.AddScoped<IRecipeReadOnlyRespository, RecipeRepository>();
;        }

        ///Tem que instalar a biblioteca FluentMigrator.Runner
        private static void AddFluentMigration_MySql(IServiceCollection services, IConfiguration configuration)
        {
            //Extrai a string de conexão do arquivo de configuração.
            var conectionString = configuration.ConnectionString();
            
            //Adiciona o FluentMigrator ao sistema de injeção de dependências da aplicação.
            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options

                //Configura o FluentMigrator para trabalhar com o MySQL.
                .AddMySql5()
                .WithGlobalConnectionString(conectionString)
                .ScanIn(Assembly.Load("MyRecipeBook.Infrastruture")).For.All(); 
                //Essa linha acima diz ao FluentMigrator para escanear o assembly especificado (no caso, "MyRecipeBook.Infrastruture")
                //em busca de todas as classes de migração. Essas classes são aquelas que herdam de Migration e contêm as instruções
                //de migração, como Up() e Down()
            });   
        }

        private static void AddFluentMigration_SqlServer(IServiceCollection services, IConfiguration configuration)
        {
            var conectionString = configuration.ConnectionString();

            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options
                .AddSqlServer()
                .WithGlobalConnectionString(conectionString)
                .ScanIn(Assembly.Load("MyRecipeBook.Infrastruture")).For.All();
            });


        }
               
        private static void AddTokens(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimesMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");        
            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

            services.AddScoped<IAcessTokenGenerator>(option => new JwtTokenGenerator(expirationTimesMinutes, signingKey!));
            services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey));
       
        }

        private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

        private static void AddPasswordEncripter(IServiceCollection services, IConfiguration configuration)
        {
            //GetValue só funciona com o Microsoft.Extensions.Configuration.Binder
            var senhadoDoUsuario = configuration.GetValue<string>("Settings:Password:AdditionalKey");

            services.AddScoped<IPaswordEncripter>(option => new Sha512Encripter(senhadoDoUsuario!));

        }
    }

}
