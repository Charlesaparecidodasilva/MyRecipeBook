using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Builders.Create.Table;

namespace MyRecipeBook.Infrastruture.Migrations.Versions
{
    public abstract class VersionBase : ForwardOnlyMigration
    {
        protected ICreateTableColumnOptionOrWithColumnSyntax CreatTable(string table)
        {
            return Create.Table(table)
              .WithColumn("Id").AsInt64().PrimaryKey().Identity()
              .WithColumn("CreatedOn").AsDateTime().NotNullable()
              .WithColumn("Active").AsBoolean().NotNullable();
        }
    }
}
