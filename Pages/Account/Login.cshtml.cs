using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Security.Claims;
using webapp.Data;
using webapp.Models;


namespace webapp.Pages.Account;

public class LoginModel : PageModel
{
    [BindProperty]
    public Login Input { get; set; }

    private readonly SignInManager<ApplicationUser> _signInManager;

    public LoginModel(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(String.Empty, "Invlaid login attempt");
                return Page();
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, false);
            if (result.Succeeded)
            {
                var claims = new Claim[] {
                    new Claim("amr","pwd"),
                    new Claim("SignedIn", "1")
                };

                var roles = await _signInManager.UserManager.GetRolesAsync(user);

                if (roles.Any())
                {
                    var roleClaim = string.Join(",", roles);
                    claims.Append(new Claim("Roles", roleClaim));
                }
                await _signInManager.SignInWithClaimsAsync(user, Input.RememberMe, claims);
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

}
