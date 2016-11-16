using System.Data.Entity;
using AXA.Middleware.API.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AXA.Middleware.API
{
    public class AxaContext : IdentityDbContext<IdentityUser>
    {
        public AxaContext()
            : base("AxaContext")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>()
                .ToTable("Clients");

            modelBuilder.Entity<IdentityRole>()
                .ToTable("Roles");

            modelBuilder.Entity<IdentityUserRole>()
                .ToTable("ClientRoles");

            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable("ClientClaims");

            modelBuilder.Entity<IdentityUserLogin>()
                .ToTable("ClientLogins");
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Policy> Policies { get; set; }

        public static AxaContext Create()
        {
            return new AxaContext();
        }
    }
}
