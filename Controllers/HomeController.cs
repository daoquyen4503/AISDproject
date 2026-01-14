using Microsoft.AspNetCore.Mvc;
using AISDproject.Data; // chứa RestaurantContext
using System.Linq;

namespace AISDproject.Controllers
{
    public class HomeController : Controller
    {
        private readonly RestaurantContext _context;

        public HomeController(RestaurantContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var menuItems = _context.MenuItems.ToList(); // lấy tất cả món
            return View(menuItems); // truyền vào View
        }
    }
}
