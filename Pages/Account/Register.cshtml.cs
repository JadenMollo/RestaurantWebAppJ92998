using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using webapp.Data;
using webapp.Models;

namespace webapp.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public Registration Input { get; set; }

        private RestaurantContext _db;
        public CheckoutCustomer Customer = new CheckoutCustomer();
        public Basket Basket = new Basket();


        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RestaurantContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email};
                var result = await _userManager.CreateAsync(user,Input.Password);

                if (result.Succeeded)
                {
                    var claims = new Claim[] {
                    new Claim("amr","pwd"),
                    new Claim("SignedIn", "1")
                };
                    await _signInManager.SignInWithClaimsAsync(user, isPersistent: false,claims);
                    NewBasket();
                    NewCustomer(Input.Email);
                    await _db.SaveChangesAsync();

                    return RedirectToPage("/Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            return Page();
        }
        public void NewBasket()
        {
            var currentBasket = _db.Baskets.FromSqlRaw("SELECT * From Baskets")
                .OrderByDescending(b => b.BasketID)
                .FirstOrDefault();
            if (currentBasket == null)
            {
                Basket.BasketID = 1;
            }
            else
            {
                Basket.BasketID = currentBasket.BasketID + 1;
            }

            _db.Baskets.Add(Basket);
        }

        public void NewCustomer(string Email)
        {
            Customer.Email = Email;
            Customer.BasketID = Basket.BasketID;
            _db.CheckoutCustomers.Add(Customer);
        }

    }
}
