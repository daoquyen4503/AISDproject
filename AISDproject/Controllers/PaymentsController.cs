using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AISDproject.Models;
using AISDproject.Services;

namespace AISDproject.Controllers
{
    [Route("Payments")]
    public class PaymentsController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // Redirect /Payments → /Payments/Index
        [HttpGet("")]
        public IActionResult RedirectToIndex()
        {
            return RedirectToAction(nameof(Index));
        }

        // GET: /Payments/Index
        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return View(payments);
        }

        // GET: /Payments/Details/{id}
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null) return NotFound();
            return View(payment);
        }

        // GET: /Payments/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Payments/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create(Payment payment)
        {
            if (ModelState.IsValid)
            {
                await _paymentService.CreatePaymentAsync(payment);
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
        }

        // GET: /Payments/Edit/{id}
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null) return NotFound();
            return View(payment);
        }

        // POST: /Payments/Edit/{id}
        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, Payment payment)
        {
            if (id != payment.PaymentId) return BadRequest();

            if (ModelState.IsValid)
            {
                await _paymentService.UpdatePaymentAsync(payment);
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
        }

        // GET: /Payments/Delete/{id}
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null) return NotFound();
            return View(payment);
        }

        // POST: /Payments/Delete/{id}
        [HttpPost("Delete/{id}"), ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _paymentService.DeletePaymentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Payments/Confirm/{id}
        [HttpPost("Confirm/{id}")]
        public async Task<IActionResult> Confirm(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null) return NotFound();

            // Cập nhật trạng thái thanh toán
            payment.Status = PaymentStatus.Paid;
            payment.PaymentTime = DateTime.Now;

            // Optional: nếu muốn cập nhật trạng thái đơn hàng
            if (payment.Order != null)
            {
                payment.Order.Status = OrderStatus.Completed;
            }

            await _paymentService.UpdatePaymentAsync(payment);
            return RedirectToAction(nameof(Details), new { id = payment.PaymentId });
        }

    }
}
