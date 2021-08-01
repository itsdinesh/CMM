using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMM.Models;

namespace CMM.Data
{
    public class CMMEventContext : DbContext
    {
        public CMMEventContext (DbContextOptions<CMMEventContext> options)
            : base(options)
        {
        }

        public DbSet<CMM.Models.Event> Event { get; set; }
    }
}
