using CMM.Areas.Identity.Data;
using CMM.Data;
using CMM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        private static decimal concertPrice;
        private static int concertID;

        public async Task<IActionResult> Checkout(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            concertPrice = (decimal)@event.ConcertPrice;
            concertID = @event.ConcertID;
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout([Bind("PaymentID")] Payment @payment)
        {
            if (ModelState.IsValid)
            {
                // Update Ticket Purchase Counter
                var @event = await _context.Event.FindAsync(concertID);
                @event.TicketPurchased += 1;

                // Update Concert Status and Hide Event to Guest/Patron when Sold Out
                if (@event.TicketPurchased == @event.TicketLimit)
                {
                    @event.ConcertStatus = "Sold Out";
                    @event.ConcertVisibility = false;
                }

                await _context.SaveChangesAsync();

                // Update Ticket to Payment Table
                DateTime currrentDate = DateTime.Now;
                _paymentContext.Add(@payment);
                @payment.User_id = (await _userManager.GetUserAsync(User))?.Id;
                @payment.PaymentDate = currrentDate;
                _paymentContext.Add(@payment);
                @payment.PaymentPrice = concertPrice;
                @payment.ConcertID = concertID;
                await _paymentContext.SaveChangesAsync();
                return RedirectToAction(nameof(TicketHistory));
            }
            return View(@payment);
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

            int concertID = payment.ConcertID;

            var eventID = await _context.Event
              .FirstOrDefaultAsync(m => m.ConcertID == concertID);

            ViewBag.ConcertPoster = eventID.ConcertPoster;
            ViewBag.ConcertMusician = eventID.ConcertMusician;
            ViewBag.ConcertName = eventID.ConcertName;
            ViewBag.ConcertDescription = eventID.ConcertDescription;
            ViewBag.ConcertLink = eventID.ConcertLink;
            ViewBag.ConcertDateTime = eventID.ConcertDateTime;
            ViewBag.ConcertStatus = eventID.ConcertStatus;
            return View(payment);
        }

        public IActionResult EditUserProfile()
        {
            return View();
        }
    }
}
