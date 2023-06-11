using Hx.Sdk.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Sdk.NetCore.Test.EFCore
{
    public class BlogContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.AddCustomConvention();
            optionsBuilder.UseSqlServer("Server=.;Database=EFCoreTest;User ID=sa;Password=songtaojie;");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Blog>().Property(b => b.Active).HasColumnType("decimal(18,3)");
            modelBuilder.Entity<Blog>().HasData(new Blog
            {
                Id = Guid.NewGuid().ToString(),
                Active = 12M
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
