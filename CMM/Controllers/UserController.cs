using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMM.Controllers
{
    [Authorize(Roles = "Patron")]
    public class UserController : Controller
    {
        public IActionResult UserViewUpcomingEvents()
        {
            return View();
        }

        public IActionResult PurchaseTicket()
        {
            return View();
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
