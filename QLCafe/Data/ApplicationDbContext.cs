using System.Collections.Generic;
using System;
using QLCafe.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;

namespace QLCafe.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Topping> Toppings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<OrderDetailTopping> OrderDetailToppings { get; set; }
        public DbSet<CartItemTopping> CartItemToppings { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Quan hệ 1-n giữa User và Order
            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ Category - Drink
            builder.Entity<Drink>()
                .HasOne(d => d.Category)
                .WithMany(c => c.Drinks)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ Drink - Review
            builder.Entity<Review>()
                .HasOne(r => r.Drink)
                .WithMany(d => d.Reviews)
                .HasForeignKey(r => r.DrinkId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ User - Review
            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ User - Cart (1-1)
            builder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ CartItem - Cart
            builder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ CartItem - Drink
            builder.Entity<CartItem>()
                .HasOne(ci => ci.Drink)
                .WithMany(d => d.CartItems)
                .HasForeignKey(ci => ci.DrinkId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartItem>()
                .Property(ci => ci.ToppingPrice)
                .HasPrecision(18, 2);

            builder.Entity<Drink>()
                .Property(d => d.Price)
                .HasPrecision(10, 2); // 10 chữ số, 2 chữ số sau dấu thập phân

            builder.Entity<Topping>()
                .Property(t => t.Price)
                .HasPrecision(10, 2);

            builder.Entity<CartItemTopping>()
                .Property(c => c.ToppingPrice)
                .HasPrecision(18, 2);

            // Quan hệ OrderDetail - Order
            builder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ OrderDetail - Drink
            builder.Entity<OrderDetail>()
                .HasOne(od => od.Drink)
                .WithMany(d => d.OrderDetails)
                .HasForeignKey(od => od.DrinkId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ Bill - Order (1-1 hoặc 1-n tùy thiết kế)
            builder.Entity<Bill>()
                .HasOne(b => b.Order)
                .WithOne(o => o.Bill)
                .HasForeignKey<Bill>(b => b.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ OrderDetailTopping - OrderDetail
            builder.Entity<OrderDetailTopping>()
                .HasOne(odt => odt.OrderDetail)
                .WithMany(od => od.OrderDetailToppings)
                .HasForeignKey(odt => odt.OrderDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ OrderDetailTopping - Topping
            builder.Entity<OrderDetailTopping>()
                .HasOne(odt => odt.Topping)
                .WithMany(t => t.OrderDetailToppings)
                .HasForeignKey(odt => odt.ToppingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ CartItemTopping - CartItem
            builder.Entity<CartItemTopping>()
                .HasOne(cit => cit.CartItem)
                .WithMany(ci => ci.CartItemToppings)
                .HasForeignKey(cit => cit.CartItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ CartItemTopping - Topping
            builder.Entity<CartItemTopping>()
                .HasOne(cit => cit.Topping)
                .WithMany(t => t.CartItemToppings)
                .HasForeignKey(cit => cit.ToppingId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
