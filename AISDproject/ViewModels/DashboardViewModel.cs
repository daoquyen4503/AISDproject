using Microsoft.AspNetCore.Mvc;

namespace AISDproject.ViewModels
{
    public class DashboardViewModel : Controller
    {
        public int TotalOrdersToday { get; internal set; }
        public decimal RevenueToday { get; internal set; }
        public int ActiveReservations { get; internal set; }

        public IActionResult Index()
        {
            return View();
        }
    }
}
