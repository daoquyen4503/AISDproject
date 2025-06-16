using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AISDproject.Data;
using AISDproject.Models;
using AISDproject.Services;

namespace AISDproject.Controllers
{
    public class TimeSlotsController : Controller
    {
        private readonly ITimeSlotService _service;

        public TimeSlotsController(ITimeSlotService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var timeSlots = await _service.GetAllTimeSlotsAsync();
            return View(timeSlots);
        }

        public async Task<IActionResult> Details(int id)
        {
            var timeSlot = await _service.GetByIdAsync(id);
            if (timeSlot == null)
                return NotFound();

            return View(timeSlot);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TimeSlot timeSlot)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(timeSlot);
                return RedirectToAction(nameof(Index));
            }
            return View(timeSlot);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var timeSlot = await _service.GetByIdAsync(id);
            if (timeSlot == null)
                return NotFound();

            return View(timeSlot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TimeSlot timeSlot)
        {
            if (id != timeSlot.TimeSlotId)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.UpdateAsync(timeSlot);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _service.ExistsAsync(timeSlot.TimeSlotId))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(timeSlot);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var timeSlot = await _service.GetByIdAsync(id);
            if (timeSlot == null)
                return NotFound();

            return View(timeSlot);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}