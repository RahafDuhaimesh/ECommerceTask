using ECommerceTask.Domain.Entities;
using ECommerceTask.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace ECommerceTask.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    ArabicName = "وردة الياسمين",
                    EnglishName = "Jasmine Flower",
                    Price = 3.7m
                },
                new Product
                {
                    Id = 2,
                    ArabicName = "وردة جورية",
                    EnglishName = "Joriya Rose",
                    Price = 2.2m
                },
                new Product
                {
                    Id = 3,
                    ArabicName = "وردة حمراء",
                    EnglishName = "Red Flower",
                    Price = 6.4m
                }
            );

        
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Rahaf Mohammad",
                    Email = "admin@flowers.com",
                    Username = "RahafAdmin",
                    Role = "Admin",
                    Password = "1234"

                },
                new User
                {
                    Id = 2,
                    FullName = "Momen Mohammad",
                    Email = "momen@gmail.com",
                    Username = "Momen123",
                    Role = "User",
                    Password = "1234"
                }
            );
        }



    }
}
