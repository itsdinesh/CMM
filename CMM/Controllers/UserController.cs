using CMM.Data;
using CMM.Models;
using Microsoft.AspNetCore.Authorization;
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

        public UserController(CMMEventContext context)
        {
            _context = context;
        }
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
        public async Task<IActionResult> Checkout(int id, [Bind("ConcertID,ConcertPoster,ConcertMusician,ConcertLink,ConcertName,ConcertDescription,ConcertDateTime,ConcertPrice,TicketLimit,ConcertStatus,ConcertVisibility")] Event @event)
        {
            if (id != @event.ConcertID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        public IActionResult TicketHistory()
        {
            return View();
        }

        public IActionResult EditUserProfile()
        {
            return View();
        }
    }
}
