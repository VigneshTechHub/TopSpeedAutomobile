using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSpeed.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace TopSpeed.Infrastructure.Common
{


    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<BrandModel> Brand { get; set; }
        public DbSet<VehicleTypeModel> VehicleType { get; set; }
        public DbSet<PostModel> Post { get; set; }
    }
}

