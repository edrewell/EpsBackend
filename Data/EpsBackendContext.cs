using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EpsBackend.Models;

namespace EpsBackend.Data
{
    public class EpsBackendContext : DbContext
    {
        public EpsBackendContext (DbContextOptions<EpsBackendContext> options)
            : base(options)
        {
        }

        public DbSet<EpsBackend.Models.User> User { get; set; } = default!;
    }
}
