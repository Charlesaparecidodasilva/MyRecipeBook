using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyRecipebook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastruture.DataAcess
{
    public class MyRecipeBookDbContext : DbContext
    {

        public MyRecipeBookDbContext(DbContextOptions options) : base(options) { }
        
        public DbSet<User> Users { get; set; }      
        
        public DbSet<Recipe> Recipes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyRecipeBookDbContext).Assembly);


        }   
    }
}
