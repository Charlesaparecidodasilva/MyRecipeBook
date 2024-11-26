using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipebook.Domain.Entities
{
    [Table("Instructions")]
    public class Instruction : EntityBase
    {
        public int Step {  get; set; }
        public string Text { get; set; } = string.Empty;       
        public long RecipeId { get; set; }

    }
}
