using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMM.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult CreateVirtualEvent()
        {
            return View();
        }

        public IActionResult EditVirtualEvent()
        {
            return View();
        }

        public IActionResult ListVirtualEvent()
        {
            return View();
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
