using AutoMapper;
using CommomTestUtilities.Cryptography;
using MyRecipeBook.Application.Services.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommomTestUtilities.Mapper
{
    public class MapperBuilder
    {
       public static IMapper Build()
        {
            var idEcricpter = IdEncripterBuilder.Build();

            var mappe = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping(idEcricpter));
            }).CreateMapper();

            return mappe;
        }

    }
}
