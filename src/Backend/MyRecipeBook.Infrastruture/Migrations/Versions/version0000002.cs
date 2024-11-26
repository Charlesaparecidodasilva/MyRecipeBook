using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastruture.Migrations.Versions
{

    [Migration(DatabaseVersion.TABLE_RECIPES, "Create table to save the recipe' information")]
    public class version0000002 : VersionBase
    {

        public override void Up()
        {
            CreatTable("Recipes")
                .WithColumn("Title").AsString().NotNullable()
                .WithColumn("CookingTime").AsInt32().Nullable()
                .WithColumn("Difficulty").AsInt32().Nullable()
                .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Recipe_User_Id", "Users", "Id");

            CreatTable("Ingredients")
                .WithColumn("Item").AsString().NotNullable()
                .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Ingredient_Recipe_Id", "Recipes", "Id")
                .OnDelete(System.Data.Rule.Cascade);

            CreatTable("Instructions")
                .WithColumn("Step").AsInt32().NotNullable()
                .WithColumn("Text").AsString(2000).NotNullable()
                .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Instruction_Recipe_Id", "Recipes", "Id")
                .OnDelete(System.Data.Rule.Cascade);

            CreatTable("DishTypes")
             .WithColumn("Type").AsInt32().NotNullable()             
             .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_DishType_Recipe_Id", "Recipes", "Id")
             .OnDelete(System.Data.Rule.Cascade);
        }
    }
}




