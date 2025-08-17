using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderAggregate;
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
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<OrderDelivery> OrderDeliveries { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

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
                new Product { Id = 7, Name = "Winter Jacket", Description = "Waterproof, hooded", Price = 89.99m, Stock = 50, CategoryId = 2 },
                new Product { Id = 8, Name = "AirPods", Description = "Wireless Bluetooth earbuds with microphone", Price = 95.99m, Stock = 155, CategoryId = 1 },
                new Product { Id = 9, Name = "Star Shoes", Description = "Casual star-patterned sports shoes", Price = 195.99m, Stock = 114, CategoryId = 5},
                new Product { Id = 10, Name = "Smartphone", Description = "128GB storage, dual camera", Price = 599.99m, Stock = 70, CategoryId = 1 },
                new Product { Id = 11, Name = "Smartwatch", Description = "Fitness tracking, heart rate monitor", Price = 199.99m, Stock = 45, CategoryId = 1 },
                new Product { Id = 12, Name = "Gaming Mouse", Description = "RGB lighting, 16000 DPI", Price = 49.99m, Stock = 120, CategoryId = 1 },
                new Product { Id = 13, Name = "Hoodie", Description = "Fleece, size L", Price = 34.99m, Stock = 65, CategoryId = 2 },
                new Product { Id = 14, Name = "Jeans", Description = "Denim, slim fit", Price = 44.99m, Stock = 90, CategoryId = 2 },
                new Product { Id = 15, Name = "Running Shoes", Description = "Lightweight, breathable mesh", Price = 75.99m, Stock = 55, CategoryId = 5 },
                new Product { Id = 16, Name = "Dining Table", Description = "Wooden, seats six", Price = 349.99m, Stock = 15, CategoryId = 4 },
                new Product { Id = 17, Name = "Bookshelf", Description = "5-tier wooden rack", Price = 129.99m, Stock = 25, CategoryId = 4 },
                new Product { Id = 18, Name = "Sofa", Description = "3-seater, fabric upholstery", Price = 499.99m, Stock = 10, CategoryId = 4 },
                new Product { Id = 19, Name = "Yoga Mat", Description = "Non-slip, 6mm thick", Price = 19.99m, Stock = 100, CategoryId = 5 },
                new Product { Id = 20, Name = "Football", Description = "FIFA quality certified", Price = 29.99m, Stock = 40, CategoryId = 5 }                
                );
        }
    }
}
