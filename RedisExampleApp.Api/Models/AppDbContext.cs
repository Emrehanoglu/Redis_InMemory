using Microsoft.EntityFrameworkCore;

namespace RedisExampleApp.Api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Kitap", Price = 50 },
                new Product { Id = 1, Name = "Kalem", Price = 50 },
                new Product { Id = 1, Name = "Silgi", Price = 50 });


            base.OnModelCreating(modelBuilder);
        }
    }
}