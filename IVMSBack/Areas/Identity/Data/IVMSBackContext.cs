using IVMSBack.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IVMSBack.Models
{
    public class IVMSBackContext : IdentityDbContext<IVMSBackUser>
    {
        public IVMSBackContext(DbContextOptions<IVMSBackContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<IVMSBackUser> IVMSBackUser { get; set; }
        public DbSet<IVMSBackRole> IVMSBackRole { get; set; }
        public DbSet<IVMSBackUserLines> IVMSBackUserLines { get; set; }
        public DbSet<Line> Line { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<VehicleLines> VehicleLines { get; set; }
        public DbSet<Origin> Origin { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<IVMSBackUserStatus> IVMSBackUserStatus { get; set; }
        public DbSet<IVMSBackUserStatusStore> IVMSBackUserStatusStore { get; set; }
        public DbSet<VehicleStatus> VehicleStatus { get; set; }
        public DbSet<VehicleStatusStore> VehicleStatusStore { get; set; }
        public DbSet<IVMSBackUserVehicles> IVMSBackUserVehicles { get; set; }
    }
}
