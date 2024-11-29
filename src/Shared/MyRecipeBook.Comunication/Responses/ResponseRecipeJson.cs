using MyRecipebook.Comunication.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Comunication.Responses
{
    public class ResponseRecipeJson
    {
        public string Id {  get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public IList<ResponseIngredientJson> Ingrediente { get; set; } = [];
        
        public IList<ResponseInstructionJson> instruction { get; set; } = [];

        public IList<DishType> DishTypes { get; set; } = [];

        public CookingTime? CookingTime { get; set; }

        public Difficulty Difficulty { get; set; }  

       
    }
}
