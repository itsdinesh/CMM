using CMM.Areas.Identity.Data;
using CMM.Data;
using CMM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CMM.Controllers
{
    [Authorize(Roles = "Patron")]
    public class UserController : Controller
    {
        private readonly CMMEventContext _context;
        private readonly CMMNewContext _paymentContext;
        private readonly UserManager<CMMUser> _userManager;

        public UserController(CMMEventContext contextEvent, CMMNewContext contextPayment, UserManager<CMMUser> userManager)
        {
            _context = contextEvent;
            _paymentContext = contextPayment;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<string> GetCurrentUserId()
        {
            CMMUser usr = await GetCurrentUserAsync();
            return usr?.Id;
        }

        private Task<CMMUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public async Task<IActionResult> UserViewUpcomingEvents()
        {
            return View(await _context.Event.ToListAsync());
        }

        public async Task<IActionResult> PurchaseTicket(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.ConcertID == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        public async Task<IActionResult> Checkout(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout([Bind("ConcertID,ConcertPoster,ConcertMusician,ConcertLink,ConcertName,ConcertDescription,ConcertDateTime,ConcertPrice,TicketLimit,ConcertStatus,ConcertVisibility")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }
        public async Task<IActionResult> TicketHistory()
        {
            return View(await _paymentContext.Payment.ToListAsync());
        }

        public async Task<IActionResult> TicketDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var payment = await _paymentContext.Payment
                .FirstOrDefaultAsync(m => m.PaymentID == id);
            if (payment == null)
            {
                return NotFound();
            }

            string userIDLoggedin = (await _userManager.GetUserAsync(User))?.Id;
            string currentPaymentUserID = payment.User_id;

            if (currentPaymentUserID != userIDLoggedin) 
            {
                return NotFound();
            }

            return View(payment);
        }

        public IActionResult EditUserProfile()
        {
            return View();
        }
    }
}
