using Microsoft.AspNetCore.Mvc;
using AISDproject.Data; // chứa RestaurantContext
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AISDproject.Controllers
{
    public class HomeController : Controller
    {
        private readonly RestaurantContext _context;

        public HomeController(RestaurantContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .Include(c => c.MenuItems)
                .ToListAsync();
            return View(categories);
        }

    }
}
