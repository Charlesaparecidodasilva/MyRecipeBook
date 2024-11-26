
using FluentMigrator;

namespace MyRecipeBook.Infrastruture.Migrations.Versions
{
    [Migration(DatabaseVersion.TABLE_USER, "Create table to save the user´s information")]

    public class Version0000001 : VersionBase

    {
        public override void Up()
        {
            CreatTable("Users")
              .WithColumn("Name").AsString(255).NotNullable()
              .WithColumn("Email").AsString(255).NotNullable()
              .WithColumn("Password").AsString(2000).NotNullable()
              .WithColumn("UserIdentifier").AsGuid().NotNullable();
        }

    }
}

