using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipebook.Domain.Entities
{
    [Table("DishTypes")]
    public class DishType : EntityBase
    {
        public Enum.DishType Type { get; set; }

        public long  RecipeId {  get; set; }  
    }
}
