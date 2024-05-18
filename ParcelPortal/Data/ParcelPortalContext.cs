using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParcelPortal.Models;


namespace ParcelPortal.Data
{
    public class ParcelPortalContext : DbContext
    {
        public ParcelPortalContext(DbContextOptions<ParcelPortalContext> options)
            : base(options)
        {
        }

        public DbSet<ParcelPortal.Models.User> User { get; set; }
        public DbSet<ParcelPortal.Models.Branch> Branch { get; set; }
        public DbSet<ParcelPortal.Models.UserRoles> UserRoles { get; set; }
        public DbSet<ParcelPortal.Models.Courier> Courier { get; set; }

    }
}
