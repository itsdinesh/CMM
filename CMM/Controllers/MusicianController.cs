using CMM.Areas.Identity.Data;
using CMM.Data;
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
    [Authorize(Roles = "Musician")]
    public class MusicianController : Controller
    {
        private readonly CMMEventContext _context;
        private readonly UserManager<CMMUser> _userManager;

        public MusicianController(CMMEventContext contextEvent, UserManager<CMMUser> userManager)
        {
            _context = contextEvent;
            _userManager = userManager;
        }

        public async Task<IActionResult> MusicianViewUpcomingEvents()
        {
            string concertMusician = (await _userManager.GetUserAsync(User))?.Name;
            ViewBag.concertMusician = concertMusician;

            return View(await _context.Event.ToListAsync());
        }

        public IActionResult MusicianEditAccount()
        {
            return View();
        }

        public async Task<IActionResult> MusicianEventDetails(int? id)
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
    }
}
