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

            modelBuilder.Entity<Review>().HasKey(x => new { x.AuthorId, x.ItemId });

            modelBuilder.Entity<Item>().HasOne(x => x.Genre).WithMany(x => x.Items).HasForeignKey(x => x.GenreId);

            modelBuilder.Entity<Item>().HasMany(x => x.Reviews).WithOne(x => x.Item).HasForeignKey( x => x.ItemId );

            modelBuilder.Entity<Review>().HasOne(x => x.Item).WithMany(x => x.Reviews).HasForeignKey(x => x.ItemId);

            modelBuilder.Entity<Review>().HasOne(x => x.Author); 

            modelBuilder.Seed(); 
       
        }


        public DbSet<Item> Items { get; set; }
        public DbSet<Genre> Genres { get; set;  }
        public DbSet<Review> Reviews { get; set;  }
       
    }
}
