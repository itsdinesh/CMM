using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMM.Models
{
    public class UserLogin: TableEntity
    {
        public UserLogin(string PartitionKey, string RowKey)
        {
            this.PartitionKey = PartitionKey;
            this.RowKey = RowKey;
        }
        public UserLogin() { }

        //create other properties
        public string Email { get; set; }
        public string Action { get; set; }
    }
}
