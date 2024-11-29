using Sqids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Cryptography
{
    public class IdEncripterBuilder
    {

        public static SqidsEncoder<long> Build()
        {

            return new SqidsEncoder<long>(new()
            {
                MinLength = 3,
                Alphabet = "MXoOilmeWjcGANEFhfspdaQtULPngxRyYrDbuCJqzvwkBKZSHITV"

            });                                        
        }
    }
}
