
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using simple_seller_app.Contexts.Tables;
using simple_seller_app.Models;

namespace simple_seller_app.Contexts
{
    public class DbSellerContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public DbSellerContext() { }
        public DbSellerContext(DbContextOptions options) : base(options) { }
        protected readonly IConfiguration configuration;

        public DbSet<m_product> m_product { get; set; }
        public DbSet<t_transaction> t_transaction { get; set; }
        public DbSet<u_user> u_user { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<m_product>().Property(b => b.created_date).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<t_transaction>().Property(b => b.created_date).HasDefaultValueSql("GETDATE()");

            // Mark IdentityUserLogin as keyless
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasNoKey();
            });

            // Mark IdentityUserRole as keyless
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasNoKey();
            });

            // Mark IdentityUserToken as keyless
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.HasNoKey();
            });
        }
    }
}