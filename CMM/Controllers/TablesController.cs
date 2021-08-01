using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CMM.Models;

namespace CMM.Controllers
{
    public class TablesController : Controller
    {
        private CloudTable getContainerInformation()
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

            //1.3: Create client object to refer to the correct table.
            //step 2: how to create a new table in the table storage account.
            CloudTableClient clientagent = accountdetails.CreateCloudTableClient();
            CloudTable table = clientagent.GetTableReference("Logs");

            return table;
        }

        public void testFunction(string PartitionKey, string RowKey, string Role, string Action)
        {
            UserLogin log = new UserLogin(PartitionKey, RowKey);
            log.Role = Role;
            log.Action = Action;


            CloudTable table = getContainerInformation();
            try
            {
                TableOperation insertOperation = TableOperation.Insert(log); // Create Action
                TableResult insertResult = table.ExecuteAsync(insertOperation).Result; // Run Action & Get Result
                ViewBag.result = insertResult.HttpStatusCode; // Get the Network Success Code = 204
                ViewBag.TableName = table.Name;
            }
            catch (Exception ex)
            {
                ViewBag.result = 100; // Create own error code.
                ViewBag.message = "Error: " + ex.ToString();
            }
        }
    }
}
