﻿using Microsoft.AspNetCore.Mvc;
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

namespace CMM.Controllers
{
    public class AdminController : Controller
    {
        private readonly CMMContext _context;

        public AdminController(CMMContext context)
        {
            _context = context;
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

        // 2. How to use form data to upload a file.
        public ActionResult CreateVirtualEvent(string Message = null)
        {
            ViewBag.msg = Message;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVirtualEvent(List<IFormFile> files, [Bind("ConcertID,ConcertPoster,ConcertMusician,ConcertLink,ConcertName,ConcertDescription,ConcertDateTime,ConcertPrice,TicketLimit,ConcertStatus,ConcertVisibility")] Event @event)
        {
            CloudBlobContainer container = getBlobContainerInformation();
            CloudBlockBlob blobitem = null;
            string message = null;

            foreach (var file in files)
            {
                try
                {
                    blobitem = container.GetBlockBlobReference(file.FileName);
                    var stream = file.OpenReadStream();
                    blobitem.UploadFromStreamAsync(stream).Wait();
                    message += "The " + blobitem.Name + blobitem.Uri + " has been successfully uploaded the blob storage.\\n";
                    ViewBag.BlobUri = blobitem.Uri;
                }
                catch (Exception ex)
                {
                    message = "The file of " + blobitem.Name + " is not able to be uploaded to the blob storage.\\n";
                    message += "Error Reason: " + ex.ToString();
                }
            }

            try
            {
               // _context.Add(@event.ConcertPoster = "visualstudio");
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                message += "Error Reason: " + ex.ToString();
            }

            return RedirectToAction("UploadFileFromForm", "Blobs", new { Message = message });
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
