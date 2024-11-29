
using Bogus;

using MyRecipebook.Comunication.Enum;
using MyRecipeBook.Comunication.Request;

namespace CommomTestUtilities.Request
{
    public class RequestFilterRecipeJsonBuilder
    {
        public static RequestFilterRecipeJson Build()
        {
            return new Faker<RequestFilterRecipeJson>()
                .RuleFor(filter => filter.RecipeTitle_Ingredientes, fake => fake.Lorem.Word())
                .RuleFor(filter => filter.CookingTimes, fake => fake.Make(3, () => fake.PickRandom<CookingTime>()).Distinct().ToList())
                .RuleFor(filter => filter.Difficulties, fake => fake.Make(3, () => fake.PickRandom<Difficulty>()).Distinct().ToList())
                .RuleFor(filter => filter.DishTypes, fake => fake.Make(3, () => fake.PickRandom<DishType>()).Distinct().ToList());
        }
    }
}
