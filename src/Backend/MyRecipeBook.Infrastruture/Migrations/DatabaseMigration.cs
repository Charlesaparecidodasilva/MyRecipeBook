using Dapper;
using FluentMigrator.Runner;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using MyRecipebook.Domain.Enum;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastruture.Migrations
{
    public class DatabaseMigration
    {
        public static void Migrate(DatabaseType TipoDobanco, string StringConexão, IServiceProvider serviceProvider)
        {
            if (TipoDobanco == DatabaseType.MySql)
            {
                EnsurDataBaseCreated_MySql(StringConexão);              
            }
            else
            {
                EnsurDataBaseCreated_Sql(StringConexão);
            }
            MigrationDataBase(serviceProvider);
        }

        private static void EnsurDataBaseCreated_MySql(string StringConexão)
        {
            //E nessesário instalar o Dapper para usar o "DynamicParameters" e o "Query";

            //Esse ConnectionStringBuilder pega as informações da string de conexao e joga na variável StringConexãoConstrutora. 

            var StringConexãoConstrutora = new MySqlConnectionStringBuilder(StringConexão);

            var nomedoBanco = StringConexãoConstrutora.Database;

            StringConexãoConstrutora.Remove("Database");

            using var dbConnexão = new MySqlConnection(StringConexãoConstrutora.ConnectionString);

            var parameter = new DynamicParameters();

            parameter.Add("name", nomedoBanco);

            var record = dbConnexão.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name", parameter);


            if (record.Any() == false)
            {
                dbConnexão.Execute($"CREATE DATABASE {nomedoBanco}");
            }
        }

        private static void EnsurDataBaseCreated_Sql(string StringConexão)
        {
            var StringConexãoConstrutora = new SqlConnectionStringBuilder(StringConexão);

            var nomedoBanco = StringConexãoConstrutora.InitialCatalog;

            StringConexãoConstrutora.Remove("Database");

            using var dbConnexão = new SqlConnection(StringConexãoConstrutora.ConnectionString);

            var parameter = new DynamicParameters();

            parameter.Add("name", nomedoBanco);

            var record = dbConnexão.Query("SELECT * FROM sys.databases WHERE name = @name", parameter);


            if (record.Any() == false)
            {
                dbConnexão.Execute($"CREATE DATABASE {nomedoBanco}");
            }
        }

        private static void MigrationDataBase(IServiceProvider serviceProvider)
        {
            //Esse IMigrationRunner aqui representa a quele serviço configurado nos metodos  AddFluentMigratorCore().ConfigureRunner que esta dentro do conteimer de dependencias.
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.ListMigrations();

            runner.MigrateUp();
        }
    }
}
