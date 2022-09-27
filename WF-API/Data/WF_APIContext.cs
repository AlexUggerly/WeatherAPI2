using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WF_API.Models;

namespace WF_API.Data
{
    public class WF_APIContext : DbContext
    {
        public WF_APIContext (DbContextOptions<WF_APIContext> options)
            : base(options)
        {
        }

        public DbSet<WF_API.Models.Forecast> Forecast { get; set; } = default!;
    }
}
