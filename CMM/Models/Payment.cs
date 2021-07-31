using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMM.Models
{
    public class Payment //make table structure, table name - payment
    
    {
        //here define the column we need //alsop use it for making form structure
        public int PaymentID { get; set; }

        public string PatronName { get; set; }

        public Decimal PaymentPrice { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
