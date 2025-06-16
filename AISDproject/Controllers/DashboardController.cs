using AISDproject.Data;
using AISDproject.Models;
using AISDproject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class DashboardController : Controller
{
    private readonly RestaurantContext _context;

    public DashboardController(RestaurantContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;

        var viewModel = new DashboardViewModel
        {
            TotalOrdersToday = await _context.Orders
                .CountAsync(o => o.CreatedAt.Date == today),

            RevenueToday = await _context.Orders
                .Where(o => o.CreatedAt.Date == today && o.Status == OrderStatus.Completed)
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0,

            ActiveReservations = await _context.Reservations
                .CountAsync(r => r.ReservationDate.Date == today && r.Status == ReservationStatus.Confirmed)
        };

        return View(viewModel);
    }
}
