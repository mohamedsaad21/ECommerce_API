using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace ECommerce.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(ProductConfig).Assembly);

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" },
                new Category { Id = 3, Name = "Books" },
                new Category { Id = 4, Name = "Furniture" },
                new Category { Id = 5, Name = "Sports" }
                );
            builder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "15-inch display, 8GB RAM", Price = 799.99m, Stock = 40, CategoryId = 1 },
                new Product { Id = 2, Name = "Headphones", Description = "Noise-cancelling wireless", Price = 129.99m, Stock = 60, CategoryId = 1 },
                new Product { Id = 3, Name = "T-Shirt", Description = "Cotton, size M", Price = 19.99m, Stock = 80, CategoryId = 2 },
                new Product { Id = 4, Name = "Office Chair", Description = "Ergonomic, adjustable height", Price = 149.99m, Stock = 30, CategoryId = 4 },
                new Product { Id = 5, Name = "Basketball", Description = "Official size and weight", Price = 25.99m, Stock = 25, CategoryId = 5 },
                new Product { Id = 6, Name = "Fiction Novel", Description = "300 pages, best-seller", Price = 12.49m, Stock = 85, CategoryId = 3 },
                new Product { Id = 7, Name = "Winter Jacket", Description = "Waterproof, hooded", Price = 89.99m, Stock = 50, CategoryId = 2 }
                );
        }
    }
}
