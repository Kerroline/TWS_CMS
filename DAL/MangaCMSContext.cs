using MangaCMS.Models;
using MangaCMS.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.DAL
{
    public class MangaCMSContext : DbContext
    {
        public DbSet<CustomUser> Users { get; set; }

        public MangaCMSContext(DbContextOptions<MangaCMSContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
