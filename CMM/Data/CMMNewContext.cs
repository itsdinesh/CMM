using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMM.Models;

namespace CMM.Data
{
    public class CMMNewContext : DbContext
    {
        public CMMNewContext (DbContextOptions<CMMNewContext> options)
            : base(options)
        {
        }

        public DbSet<CMM.Models.Payment> Payment { get; set; }
    }
}
