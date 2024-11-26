
using Microsoft.EntityFrameworkCore;
using simple_seller_app.Contexts.Tables;

namespace simple_seller_app.Contexts
{
    public class DbSellerContext : DbContext
    {
        public DbSellerContext() { }
        public DbSellerContext(DbContextOptions options) : base(options) { }
        protected readonly IConfiguration configuration;

        // public DbSellerContext(IConfiguration _configuration)
        // {
        //     configuration = _configuration;
        // }

        // protected override void OnConfiguring(DbContextOptionsBuilder options)
        // {
        //     string connectionString = configuration["ConnectionStrings:SqlServer"]!;
        //     options.UseSqlServer(connectionString);
        // }

        public DbSet<m_product> m_product { get; set; }
        public DbSet<t_transaction> t_transaction { get; set; }
        public DbSet<u_user> u_user { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<m_product>().Property(b => b.created_date).HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<t_transaction>().Property(b => b.created_date).HasDefaultValueSql("GETDATE()");
        }
    }
}