//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.DependencyInjection;
//using BankingSystem.DB;
//using BankingSystem.DB.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace BankingSystem.Seeding
//{
//    public class OperatorSeed
//    {
//        public static async Task AddUserAndRoles(IApplicationBuilder builder)
//        {
//            using (var scope = builder.ApplicationServices.CreateScope())
//            {
//                var service = scope.ServiceProvider;
//                var userContext = service.GetRequiredService<AppDbContext>();
//                var userManager = service.GetRequiredService<UserManager<UserEntity>>();
//                var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

//                Migrate(userContext);
//                await SeedRolesAsync(roleManager);
//                await SeedOperatorAsync(userManager, roleManager);
//            }
//        }

//        private static void Migrate(AppDbContext context)
//        {
//            context.Database.Migrate();
//        }
//        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
//        {
//            await roleManager.CreateAsync(new IdentityRole("api-operator"));
//            await roleManager.CreateAsync(new IdentityRole("api-user"));
//        }

//        private static async Task SeedOperatorAsync(UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager)
//        {
//            var newOperator = new UserEntity()
//            {
//                FirstName = "Lasha",
//                LastName = "Avalishvili",
//                UserName = "Operator",
//                Email = "lasha@gmail.com",
//                PersonalNumber = "19001108016",
//                DateOfBirth = DateTime.Parse("1999/04/04"),
//                RegisteredAt = DateTime.Now,
//                EmailConfirmed = true,
//                PhoneNumberConfirmed = true
//            };
//            if (userManager.Users.All(u => u.Id != newOperator.Id))
//            {
//                var user = await userManager.FindByEmailAsync(newOperator.Email);
//                if (user == null)
//                {
//                    await userManager.CreateAsync(newOperator, "12345678");
//                    await userManager.AddToRoleAsync(newOperator, "api-operator");
//                }
//            }
//        }
//    }
//}

