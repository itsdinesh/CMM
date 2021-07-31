using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.AspNetCore.Http;

namespace BlobStorageExample.Controllers
{
    public class BlobsController : Controller
    {
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
        public ActionResult UploadFileFromForm(string Message = null)
        {
            ViewBag.msg = Message;
            return View();
        }

        [HttpPost]
        public ActionResult UploadFileFromForm(List<IFormFile> files)
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
                    message += "The " + blobitem.Name + " has been successfully uploaded the blob storage.\\n";
                }
                catch (Exception ex)
                {
                    message = "The file of " + blobitem.Name + " is not able to be uploaded to the blob storage.\\n";
                    message += "Error Reason: " + ex.ToString();
                }
            }
            return RedirectToAction("UploadFileFromForm", "Blobs", new { Message = message });
        }

        // 3. Learn how to view blob items in a page.
        public ActionResult ListBlobsAsGallery(string Message = null)
        {
            ViewBag.msg = Message;
            CloudBlobContainer container = getBlobContainerInformation();

            // Create empty list
            List<string> bloblist = new List<String>();

            // Get the blob result from Storage
            BlobResultSegment listing = container.ListBlobsSegmentedAsync(null).Result;

            // Read blob by blob from listing
            foreach (IListBlobItem item in listing.Results)
            {
                // Check the blob type (Block Blob / Page Blob / Directory Blob Append Blob / )
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    if (Path.GetExtension(blob.Name) == ".jpg" || Path.GetExtension(blob.Name) == ".png") 
                    {
                        bloblist.Add(blob.Name + "#" + blob.Uri); // # is delimiter
                    }
                }
            }
            return View(bloblist);
        }

        // 6. Learn how to delete a blob from storage
        public ActionResult deleteblob(string imagename)
        {
            string message = null;
            CloudBlobContainer container = getBlobContainerInformation();
            try
            {
                CloudBlockBlob item = container.GetBlockBlobReference(imagename);
                string name = item.Name;
                item.DeleteIfExistsAsync().Wait();
                message = "The blob of " + name + " is deleted from blob storage.";
            }
            catch (Exception ex)
            {
                message = "The selected blob " + imagename + "is unable to delete. Reason: " + ex;
            }

            return RedirectToAction("ListBlobsAsGallery", "Blobs", new { Message = message });
        }

        // 7. Learn how to download blob from storage to Client PC.
        public async Task<ActionResult> downloadblob(string imagename) 
        {
            string message = null;
            CloudBlobContainer container = getBlobContainerInformation();
            CloudBlockBlob blob;

            try 
            {
                //CloudBlockBlob item = container.GetBlockBlobReference(imagename);
                //var outputitem = System.IO.File.OpenWrite(@"C:\\Users\Dinesh\\Desktop" + imagename);
                //item.DownloadToStreamAsync(outputitem).Wait();
                //message = imagename + "has been download to your desktop";
                //outputitem.Close();

                await using (MemoryStream memoryStream = new MemoryStream()) 
                {
                    blob = container.GetBlockBlobReference(imagename);
                    await blob.DownloadToStreamAsync(memoryStream);
                }
                Stream blobStream = blob.OpenReadAsync().Result;
                return File(blobStream, blob.Properties.ContentType, blob.Name);
            }
            catch (Exception ex)
            {
                message = "The selected blob " + imagename + "is unable to download. Reason: " + ex;
            }

            return RedirectToAction("ListBlobsAsGallery", "Blobs", new { Message = message });
        }

    }
}
