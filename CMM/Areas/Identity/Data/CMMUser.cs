using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CMM.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the CMMUser class
    public class CMMUser : IdentityUser //modify the table column
    {
        //add extra information such as: name, age, dob, address

        [PersonalData]
        public string Name { get; set; }

        [PersonalData]
        public int Age { get; set; }

        [PersonalData]
        public string Gender { get; set; }

        [PersonalData]
        public string GroupName { get; set; }

        [PersonalData]
        public string Address { get; set; }

        [PersonalData]
        public string userRoles { get; set; }

    }
}
