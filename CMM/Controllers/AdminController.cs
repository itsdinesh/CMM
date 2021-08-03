using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMM.Data;
using CMM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using CMM.Areas.Identity.Data;

namespace CMM.Controllers
{
    [Authorize(Roles = "Manager")]
    public class AdminController : Controller
    {
        private readonly CMMEventContext _context;
        private readonly UserManager<CMMUser> _userManager;

        public AdminController(CMMEventContext context, UserManager<CMMUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 1. Create New Function - Link to the blob storage account & also at the correct container.
        private CloudBlobContainer getBlobContainerInformation()
        {
            // 1.1: Link with the appsettings.json 
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
            IConfiguration configure = builder.Build();

            //1.2: Get the access key connection string so that the app links to the correct storage account.
            //once link, time to read the content to get the connectionstring
            CloudStorageAccount accountdetails = CloudStorageAccount.
                Parse(configure["ConnectionStrings:blobstorageconnection"]);

            //1.3: Create client object to refer to the correct container.
            //step 2: how to create a new container in the blob storage account.
            CloudBlobClient clientagent = accountdetails.CreateCloudBlobClient();
            CloudBlobContainer container = clientagent.GetContainerReference("eventimages");
            return container;
        }

        public List<SelectListItem> MusicianNames { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "MX", Text = "Mexico" },
            new SelectListItem { Value = "CA", Text = "Canada" },
            new SelectListItem { Value = "US", Text = "USA"  },
        };

        // 2. How to use form data to upload a file.
        public IActionResult CreateVirtualEvent(string Message = null)
        {
            ViewBag.MusicianList = MusicianNames;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVirtualEvent(List<IFormFile> files, [Bind("ConcertID,ConcertPoster,ConcertMusician,ConcertLink,ConcertName,ConcertDescription,ConcertDateTime,ConcertPrice,TicketLimit,ConcertStatus,ConcertVisibility")] Event @event)
        {
            CloudBlobContainer container = getBlobContainerInformation();
            // 3.2: Give the upload blob name
            CloudBlockBlob blobitem = null;
            string message = null;

            // Find Last ID
            var showPiece = _context.Event.OrderByDescending(p => p.ConcertID).FirstOrDefault();
            int lastID;

            if (showPiece == null)
            {
                lastID = 1;
            }
            else
            {
                lastID = showPiece.ConcertID + 1;
            }

            foreach (var file in files)
            {
                try
                {
                    blobitem = container.GetBlockBlobReference("eventpic_" + lastID + ".png");
                    var stream = file.OpenReadStream();
                    blobitem.UploadFromStreamAsync(stream).Wait();
                }
                catch (Exception ex)
                {
                    message = "The file of " + blobitem.Name + " is not able to be uploaded to the blob storage.\\n";
                    message += "Error Reason: " + ex.ToString();
                }
            }
            @event.ConcertPoster = blobitem.Uri.ToString();
            @event.TicketPurchased = 0;
            _context.Add(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListVirtualEvent));
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
        public async Task<IActionResult> EditVirtualEvent(int id, List<IFormFile> files, [Bind("ConcertID,ConcertPoster,ConcertMusician,ConcertLink,ConcertName,ConcertDescription,ConcertDateTime,ConcertPrice,TicketLimit,TicketPurchased,ConcertStatus,ConcertVisibility")] Event @event)
        {
            if (id != @event.ConcertID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {                    
                    CloudBlobContainer container = getBlobContainerInformation();
                    // 3.2: Give the upload blob name
                    CloudBlockBlob blobitem = null;
                    string message = null;

                    foreach (var file in files)
                    {
                        try
                        {
                            blobitem = container.GetBlockBlobReference("eventpic_" + @event.ConcertID + ".png");
                            var stream = file.OpenReadStream();
                            blobitem.UploadFromStreamAsync(stream).Wait();
                        }
                        catch (Exception ex)
                        {
                            message += "The file of " + blobitem.Name + " is not able to be uploaded to the blob storage.\\n";
                            message += "Error Reason: " + ex.ToString();
                        }
                    }
                    if(blobitem == null) 
                    {
                        blobitem = container.GetBlockBlobReference("eventpic_" + @event.ConcertID + ".png");
                        @event.ConcertPoster = blobitem.Uri.ToString();
                    }
                    @event.ConcertPoster = blobitem.Uri.ToString();
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
            var users = _userManager.Users;
            return NotFound(users.ToListAsync());
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
