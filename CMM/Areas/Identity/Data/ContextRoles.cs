using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMM.Areas.Identity.Data
{
    public enum Roles
    {
        Manager,
        Musician,
        Patron
    }
    public class ContextRoles
    {
        public static async Task SeedRolesAsync(UserManager<CMMUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Roles.Manager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Musician.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Patron.ToString()));
        }
    }
}
