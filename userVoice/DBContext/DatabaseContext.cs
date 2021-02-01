using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using userVoice.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 

namespace userVoice.DBContext
{
    public class DatabaseContext : IdentityDbContext<UserEntity>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DatabaseContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasMany(c => c.Items).WithOne(a => a.Category).HasForeignKey(a => a.CategoryId);

            modelBuilder.Entity<Item>().HasOne(a => a.Category).WithMany(a => a.Items);

            modelBuilder.Entity<Item>().HasMany(a => a.Reviews).WithOne(a => a.Item).HasForeignKey(a => a.ItemId);

            modelBuilder.Entity<UserEntity>().HasMany(a => a.getReviews).WithOne(a => a.Author);

            modelBuilder.Entity<Review>().HasKey( a => new { a.ItemId, a.AuthorId } ); 

            modelBuilder.Entity<Review>().HasOne(a => a.Author).WithMany(a => a.getReviews).HasForeignKey(a => a.AuthorId);

            modelBuilder.Entity<Review>().HasOne(a => a.Item).WithMany(a => a.Reviews).HasForeignKey(a => a.ItemId);

            modelBuilder.Seed(); 
       
        }


        public DbSet<Item> items { get; set; }
        public DbSet<Category> categories { get; set;  }
        public DbSet<Review> reviews { get; set;  }
        public DbSet<SearchWord> searchWords { get; set;  }
      
    }
}
