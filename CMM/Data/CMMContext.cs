﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CMM.Models;

namespace CMM.Data
{
    public class CMMContext : DbContext
    {
        public CMMContext (DbContextOptions<CMMContext> options)
            : base(options)
        {
        }

        public DbSet<CMM.Models.Event> Event { get; set; }
    }
}
