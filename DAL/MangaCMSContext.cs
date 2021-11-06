using MangaCMS.Models;
using MangaCMS.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangaCMS.DAL
{
    public class MangaCMSContext : IdentityDbContext<CustomUser>
    {
        public MangaCMSContext(DbContextOptions<MangaCMSContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
