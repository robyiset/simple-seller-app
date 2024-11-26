
using Microsoft.EntityFrameworkCore;
using simple_seller_app.Contexts.Tables;

namespace simple_seller_app.Contexts
{
    public class DbSellerContext : DbContext
    {
        public DbSellerContext() { }
        public DbSellerContext(DbContextOptions<DbSellerContext> options) : base(options) { }

        public DbSet<m_product> m_product { get; set; }
        public DbSet<t_transaction> t_transaction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<m_product>().Property(b => b.created_date).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<t_transaction>().Property(b => b.created_date).HasDefaultValueSql("GETDATE()");
        }
    }
}