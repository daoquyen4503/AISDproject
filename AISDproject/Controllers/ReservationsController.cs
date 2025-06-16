using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AISDproject.Models;
using AISDproject.Services;
using AISDproject.Filters;

namespace AISDproject.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ITableService _tableService;
        private readonly ITimeSlotService _timeSlotService;

        public ReservationsController(
            IReservationService reservationService,
            ITableService tableService,
            ITimeSlotService timeSlotService)
        {
            _reservationService = reservationService;
            _tableService = tableService;
            _timeSlotService = timeSlotService;
        }

        [AuthorizeRole("Admin", "Manager")]
        public async Task<IActionResult> Index()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            return View(reservations);
        }

        [AuthorizeRole("Admin", "Manager")]
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null) return NotFound();
            return View(reservation);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                await _reservationService.CreateReservationAsync(reservation);
                return RedirectToAction(nameof(Create));
            }

            await PopulateDropdowns();
            return View(reservation);
        }

        [AuthorizeRole("Admin", "Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null) return NotFound();

            await PopulateDropdowns();
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("Admin", "Manager")]
        public async Task<IActionResult> Edit(int id, Reservation reservation)
        {
            if (id != reservation.ReservationId) return BadRequest();

            if (ModelState.IsValid)
            {
                await _reservationService.UpdateReservationAsync(reservation);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns();
            return View(reservation);
        }

        [AuthorizeRole("Admin", "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null) return NotFound();
            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("Admin", "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reservationService.DeleteReservationAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdowns()
        {
            var tables = await _tableService.GetAllTablesAsync();
            var timeSlots = await _timeSlotService.GetAllTimeSlotsAsync(); // ĐÚNG tên method

            ViewBag.TableList = new SelectList(tables, "TableId", "TableName");
            ViewBag.TimeSlotList = new SelectList(timeSlots, "TimeSlotId", "DisplayTime");
        }
    }
}
