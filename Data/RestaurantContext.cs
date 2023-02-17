using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using webapp.Models;

namespace webapp.Data
{
    public class RestaurantContext : IdentityDbContext<ApplicationUser>
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> options)
            : base(options)
        {
        }
        //menuItems
        public DbSet<Menu> Menu { get; set; }
        //BasketData
        public DbSet<CheckoutCustomer> CheckoutCustomers { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        //TransactionData
        public DbSet<OrderHistories> OrderHistories { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        //none SQL class for transsaction data
        [NotMapped]
        public DbSet<CheckoutItem> CheckoutItems { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Basket compound key
            builder.Entity<BasketItem>().HasKey(t => new { t.StockID, t.BasketID });
            //Tranaction compound key
            builder.Entity<OrderItem>().HasKey(t => new { t.StockID, t.OrderNo });
        }
    }
}
