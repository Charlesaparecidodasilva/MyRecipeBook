
using Microsoft.Extensions.Configuration;
using MyRecipebook.Domain.Enum;


namespace MyRecipeBook.Infrastruture.Extensions
{
    public static class ConfigurationExtension
    {
        public static bool IsUnitTestEnviroment(this IConfiguration configuration)
        {
            // Aqui esta verificando se o InMemoryTest do appsetting é true e retorna um valor boleano.
            return configuration.GetValue<bool>("InMemoryTest");        
        }
        public static DatabaseType DataBaseType(this IConfiguration configuration)
        {
            var databaseType = configuration.GetConnectionString("DataBaseType");           

            return (DatabaseType)Enum.Parse(typeof (DatabaseType), databaseType!);

        }
        public static string ConnectionString(this IConfiguration configuraçao)
        {

            var dataBaseType = configuraçao.DataBaseType();

            if (dataBaseType == MyRecipebook.Domain.Enum.DatabaseType.MySql)         
                return configuraçao.GetConnectionString("ConnectionMySQLServer");
            else
                return configuraçao.GetConnectionString("ConnectionSQLServer");

        }
    }
}
