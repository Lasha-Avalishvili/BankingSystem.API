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
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }

 
        public AppDbContext(DbContextOptions options) : base(options)
        {
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<AccountEntity>()
            .HasMany(a => a.Cards)
            .WithOne(c => c.Account)
            .HasForeignKey(c => c.AccountId);

            modelBuilder.Entity<IdentityUserRole<int>>(b =>
            {
                b.HasKey(i => new { i.UserId, i.RoleId });
            });


            modelBuilder.Entity<RoleEntity>().HasData(
               new RoleEntity { Id = 1, Name = "api-admin", NormalizedName = "API-ADMIN" },
               new RoleEntity { Id = 2, Name = "api-user", NormalizedName = "API-USER" }
               );

            var hasher = new PasswordHasher<UserEntity>();

           
            var newOperator = new UserEntity()

            {
                Id = 1,
                FirstName = "Lasha",
                LastName = "Avalishvili",
                UserName = "Lasha123",
                Email = "lasha@gmail.com",
                PersonalNumber = "19001108016",
                DateOfBirth = DateTime.Parse("1999/04/04"),
                RegisteredAt = DateTime.Now,
                NormalizedEmail= "LASHA@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
               PasswordHash = hasher.HashPassword(new UserEntity(), "password")

            };
            modelBuilder.Entity<UserEntity>().HasData(newOperator);

            modelBuilder.Entity<IdentityUserRole<int>>()
                .HasData(new IdentityUserRole<int> { UserId = 1, RoleId = 1 });

        }
    }
}
