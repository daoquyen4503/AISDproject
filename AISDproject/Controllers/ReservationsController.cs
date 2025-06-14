using AISDproject.Models;
using AISDproject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            return View(reservations);
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public async Task<IActionResult> Create()
        {
            await PopulateSelectListsAsync();
            return View();
        }

        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                await _reservationService.CreateReservationAsync(reservation);
                return RedirectToAction(nameof(Index));
            }

            await PopulateSelectListsAsync(reservation);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            await PopulateSelectListsAsync(reservation);
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reservation reservation)
        {
            if (id != reservation.ReservationId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _reservationService.UpdateReservationAsync(reservation);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _reservationService.GetReservationByIdAsync(reservation.ReservationId) == null)
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateSelectListsAsync(reservation);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _reservationService.DeleteReservationAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateSelectListsAsync(Reservation? reservation = null)
        {
            var tables = await _tableService.GetAllTablesAsync();
            ViewBag.Tables = new SelectList(tables, "TableId", "TableName", reservation?.TableId);

            var timeSlots = await _timeSlotService.GetAllAsync();
            ViewBag.TimeSlots = new SelectList(timeSlots, "TimeSlotId", "DisplayTime", reservation?.TimeSlotId);
        }
    }
}
