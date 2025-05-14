using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RogueFit.Models;

namespace RogueFit.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            :base(options) 
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.SubCategories)
                .WithOne(sc => sc.Category)
                .HasForeignKey(sc => sc.CategoryId);

            modelBuilder.Entity<SubCategory>()
                .HasMany(sc => sc.Tags)
                .WithOne(t => t.SubCategory)
                .HasForeignKey(t => t.SubCategoryId);

            modelBuilder.Entity<Tag>()
                .HasMany(t => t.Products)
                .WithOne(p => p.Tag)
                .HasForeignKey(p => p.TagId);
        }
    }
}
