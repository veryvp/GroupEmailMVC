using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace mvc.Models
{
    public class myDBContext : DbContext
    {
        public DbSet<Movie> Movie { get; set; }
        public DbSet<xiaozhang> xiaozhang { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }


}