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

        public UserManager<UserEntity> _userManager;
        public AppDbContext(DbContextOptions options, UserManager<UserEntity> userManager) : base(options)
        {
            _userManager = userManager;
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


            modelBuilder.Entity<RoleEntity>().HasData(
                new RoleEntity { Id = 1, Name = "api-operator", NormalizedName = "API-OPERATOR" },
                new RoleEntity { Id = 2, Name = "api-user", NormalizedName = "API-USER" }
                );


            modelBuilder.Entity<AccountEntity>()
            .HasMany(a => a.Cards)
            .WithOne(c => c.Account)
            .HasForeignKey(c => c.AccountId);

            var newOperator = new UserEntity()
            {
                Id = 1,
                FirstName = "Lasha",
                LastName = "Avalishvili",
                UserName = "O",
                Email = "lasha@gmail.com",
                PersonalNumber = "19001108016",
                DateOfBirth = DateTime.Parse("1999/04/04"),
                RegisteredAt = DateTime.Now,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,

            };
            //var hash = new PasswordHasher<UserEntity>();
            //modelBuilder.Entity<UserEntity>().HasData(newOperator);
          
            if (_userManager.Users.All(u => u.Id != newOperator.Id))
            {
                var user =  _userManager.FindByEmailAsync(newOperator.Email);
                if (user == null)
                {
                     _userManager.CreateAsync(newOperator, "12345678");
                     _userManager.AddToRoleAsync(newOperator, "api-operator");
                }
            }



            modelBuilder.Entity<IdentityUserRole<int>>()
                .HasData(new IdentityUserRole<int> { UserId = 1, RoleId = 1 });


        }
    }
}
