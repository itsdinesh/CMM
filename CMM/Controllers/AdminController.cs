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

        public IActionResult EditVirtualEvent()
        {
            return View();
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
    }
}
