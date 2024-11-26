using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastruture.Migrations
{
    public abstract class DatabaseVersion
    {
        public const int TABLE_USER = 1;
        public const int TABLE_RECIPES = 2;
    }
}
