using MyRecipebook.Comunication.Enum;


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
