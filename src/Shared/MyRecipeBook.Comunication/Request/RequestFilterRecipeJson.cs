using MyRecipebook.Comunication.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Comunication.Request
{
    public class RequestFilterRecipeJson
    {
        public string? RecipeTitle_Ingredientes {  get; set; }

        public IList<CookingTime> CookingTimes { get; set; } = [];

        public IList<Difficulty> Difficulties {  get; set; } = [];

        public IList<DishType> DishTypes { get; set; } = [];    
    }
}
