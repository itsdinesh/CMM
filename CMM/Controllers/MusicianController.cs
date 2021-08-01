﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMM.Controllers
{
    [Authorize(Roles = "Musician")]
    public class MusicianController : Controller
    {
        public IActionResult MusicianViewUpcomingEvents()
        {
            return View();
        }

        public IActionResult MusicianEditAccount()
        {
            return View();
        }
    }
}
