using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq.Expressions;
using webapp.Data;
using webapp.Models;

namespace webapp.Pages.Menus
{
    public class OrderModel : PageModel
    {
        int tableNo;
        private readonly RestaurantContext _db;
        private readonly UserManager<ApplicationUser> _UserManager;
        public IList<CheckoutItem> Items { get; private set; }
        public OrderHistories Order = new OrderHistories();
        public OrderModel(RestaurantContext db,UserManager<ApplicationUser> UserManager)
        {
            _db = db;
            _UserManager = UserManager;
        }
        public async Task OnGetAsync()
        {
            decimal ?price;
            var user = await _UserManager.GetUserAsync(User);
            CheckoutCustomer ?customer = await _db
            .CheckoutCustomers
            .FindAsync(user.Email);

            Items = _db.CheckoutItems.FromSqlRaw(
                "SELECT Menu.ID, Menu.Price, " +
                "Menu.Name, " +
                "BasketItems.BasketID, BasketItems.Quantity " +
                "FROM Menu INNER JOIN BasketItems " +
                "ON Menu.ID = BasketItems.StockID " +
                "WHERE BasketID = {0}", customer.BasketID
                ).ToList();
        }
        public async Task<IActionResult> OnPostBuyAsync()
        {

            var currentOrder = _db.OrderHistories
            .FromSqlRaw("SELECT * From OrderHistories")
                .OrderByDescending(b => b.OrderNo)
                .FirstOrDefault();

            if (currentOrder == null)
            {
                Order.OrderNo = 1;
            }
            else
            {
                Order.OrderNo = currentOrder.OrderNo + 1;
            }

            var user = await _UserManager.GetUserAsync(User);
            Order.Email = user.Email;
            _db.OrderHistories.Add(Order);

            try {

                CheckoutCustomer ?customer = await _db
                    .CheckoutCustomers
                    .FindAsync(user.Email);

                var basketItems =
                    _db.BasketItems
                    .FromSqlRaw("SELECT * From BasketItems " +
                    "WHERE BasketID = {0}", customer.BasketID)
                    .ToList();

                foreach (var item in basketItems)
                {
                    OrderItem oi = new OrderItem
                    {
                        OrderNo = Order.OrderNo,
                        StockID = item.StockID,
                        Quantity = item.Quantity
                    };
                    _db.OrderItems.Add(oi);
                    _db.BasketItems.Remove(item);
                }


                await _db.SaveChangesAsync();
                return RedirectToPage("/Index");
            } catch(Exception ex) { return RedirectToPage($"Error loading {ex.Message}"); }
        }

    }

}
