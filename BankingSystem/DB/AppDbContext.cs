using BankingSystem.DB.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.DB
{
    public class AppDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
    {
        

        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<CardEntity> Cards { get; set; }
        public DbSet<OperatorEntity> Operators { get; set; }
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }

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
            modelBuilder.Entity<ExchangeRateEntity>().ToTable("ExchangeRates");
            modelBuilder.Entity<AccountEntity>()
            .HasOne(a => a.User)
            .WithMany(u => u.Accounts)
            .HasForeignKey(a => a.UserId);

          //  modelBuilder.Entity<RoleEntity>().HasData(new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Administrator", NormalizedName = "ADMINISTRATOR".ToUpper() });

            modelBuilder.Entity<AccountEntity>()
            .HasMany(a => a.Cards)
            .WithOne(c => c.Account)
            .HasForeignKey(c => c.AccountId);

        //    modelBuilder.Entity<IdentityUserRole<string>>().HasData(
        //    new IdentityUserRole<string>
        //    {
        //        RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
        //        UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
        //    }
        //);
        //    modelBuilder.Entity<UserEntity>().HasData(
        //                new UserEntity
        //                {
        //                    UserName = "myuser",
        //                    NormalizedUserName = "MYUSER",
        //                    // PasswordHash = hasher.HashPassword(null, "Pa$$w0rd")
        //                }
        //            );
        }
    }
}
