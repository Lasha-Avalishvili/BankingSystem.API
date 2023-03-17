using BankingSystem.DB.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.DB
{
    public class AppDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
    {
        public readonly object CurrencyRates;

        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<CardEntity> Cards { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<OperatorEntity> Operators { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OperatorEntity>().ToTable("Operators");
            modelBuilder.Entity<UserEntity>().ToTable("Users");
            modelBuilder.Entity<RoleEntity>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles").HasKey(p => p.UserId);
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("OperatorClaims");
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("OperatorLogins").HasKey(p => p.UserId);
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("OperatorTokens").HasKey(p => p.UserId);
           // modelBuilder.Entity<CurrencyRateEntity>().ToTable("CurrencyRates");
            modelBuilder.Entity<AccountEntity>()
            .HasOne(a => a.User)
            .WithMany(u => u.Accounts)
            .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<AccountEntity>()
            .HasMany(a => a.Cards)
            .WithOne(c => c.Account)
            .HasForeignKey(c => c.AccountId);
        }
    }
}
