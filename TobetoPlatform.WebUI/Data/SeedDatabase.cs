// GÜNCELLENMİŞ KOD: TobetoPlatform.WebUI/Data/SeedDatabase.cs

using Microsoft.EntityFrameworkCore;
using TobetoPlatform.Core.Utilities.Security.Hashing; // YENİ: HashingHelper için
using TobetoPlatform.DataAccess;
using TobetoPlatform.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace TobetoPlatform.WebUI.Data
{
    public static class SeedDatabase
    {
        public static async Task Initialize(IServiceProvider services)
        {
            var context = services.GetRequiredService<TobetoPlatformDbContext>();
            await context.Database.MigrateAsync();

            if (!context.Roles.Any())
            {
                await context.Roles.AddRangeAsync(
                    new Role { Name = "Admin" },
                    new Role { Name = "User" }
                );
                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                // DEĞİŞİKLİK: HashingHelper ile standart şifreleme yapıyoruz
                HashingHelper.HazırlaPasswordHash("Password123", out byte[] passwordHash, out byte[] passwordSalt);

                var adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@example.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                };

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();

                var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
                if (adminRole != null)
                {
                    // DEĞİŞİKLİK: UserId yerine Id kullanılıyor
                    await context.UserRoles.AddAsync(new UserRole { UserId = adminUser.Id, RoleId = adminRole.Id });
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}