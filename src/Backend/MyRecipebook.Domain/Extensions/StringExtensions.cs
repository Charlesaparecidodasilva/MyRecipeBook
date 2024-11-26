using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipebook.Domain.Extensions
{
    public static class StringExtensions
    {
        public static bool NotEmpaty(this string value) => string.IsNullOrWhiteSpace(value).IsFalse();
    }
}
