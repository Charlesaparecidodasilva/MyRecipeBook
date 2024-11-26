using MyRecipebook.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipebook.Domain.Dtos
{
    public record FilterRecipesDto
    {
        public string? RecipeTitle_Ingredient {  get; init; }

        public IList<CookingTime> CookingTimes { get; init; } = [];

        public IList<Difficulty> Difficulties { get; init; } = [];

        public IList<DishType> DishTypes { get; init; } = [];
    }

    public record Pessoa(string name,  int age);
}
