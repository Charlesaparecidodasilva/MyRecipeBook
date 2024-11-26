using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipebook.Domain.Entities
{
    [Table("Ingredients")]
    public class Ingredient : EntityBase
    {
        public string Item {  get; set; } = string.Empty;

        public long RecipeId { get; set; }


    }
}
