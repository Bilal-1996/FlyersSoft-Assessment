using Microsoft.EntityFrameworkCore;

namespace FlyersSoft.Model
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions options) : base(options) { 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<UserBeneficiary> Beneficiary { get; set; }

        public DbSet<TopupOption> TopupOption { get; set; }
    }
}
