using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMM.Models
{
    public class UserLogin: TableEntity
    {
        public UserLogin(string Name, string ID)
        {
            this.PartitionKey = Name;
            this.RowKey = ID;
        }
        public UserLogin() { }

        //create other properties
        public string Role { get; set; }
        public string Action { get; set; }
    }
}
