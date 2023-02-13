using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using webapp.Models;

namespace webapp.Pages.Menus
{
    public class IndexModel : PageModel
    {
        private readonly webapp.Data.RestaurantContext _context;

        public IndexModel(webapp.Data.RestaurantContext context)
        {
            _context = context;
        }

        public IList<Menu> Menu { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Menu != null)
            {
                Menu = await _context.Menu.ToListAsync();
            }
        }
    }
}
