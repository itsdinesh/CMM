using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMM.Data;
using CMM.Models;
using Microsoft.EntityFrameworkCore;

namespace CMM.Controllers
{
    public class AdminController : Controller
    {
        private readonly CMMContext _context;

        public AdminController(CMMContext context)
        {
            _context = context;
        }

        public IActionResult CreateVirtualEvent()
        {
            return View();
        }

        public async Task<IActionResult> EditVirtualEvent(int? id)
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
        public async Task<IActionResult> EditVirtualEvent(int id, [Bind("ConcertID,ConcertPoster,ConcertMusician,ConcertLink,ConcertName,ConcertDescription,ConcertDateTime,ConcertPrice,TicketLimit,ConcertStatus,ConcertVisibility")] Event @event)
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
                    if (!EventExists(@event.ConcertID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListVirtualEvent));
            }
            return View(@event);
        }

        public async Task<IActionResult> ListVirtualEvent()
        {
            return View(await _context.Event.ToListAsync());
        }
        public async Task<IActionResult> ListHiddenVirtualEvent()
        {
            return View(await _context.Event.ToListAsync());
        }

        public IActionResult CreateMusicianAccount()
        {
            return View();
        }
        public IActionResult ListMusician()
        {
            return View();
        }

        public IActionResult AdminEditMusicianAccount()
        {
            return View();
        }

        public async Task<IActionResult> ViewEventDetails(int? id)
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

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.ConcertID == id);
        }
    }
}
