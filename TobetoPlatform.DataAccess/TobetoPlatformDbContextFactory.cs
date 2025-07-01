// --- Dosya: TobetoPlatformDbContextFactory.cs ---
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TobetoPlatform.DataAccess
{
    public class TobetoPlatformDbContextFactory : IDesignTimeDbContextFactory<TobetoPlatformDbContext>
    {
        public TobetoPlatformDbContext CreateDbContext(string[] args)
        {
            // API projesindeki appsettings.json dosyasını bulmak için yol yapılandırması
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../TobetoPlatform.API"))
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<TobetoPlatformDbContext>();
            var connectionString = configuration.GetConnectionString("TobetoDb");

            builder.UseSqlServer(connectionString);

            return new TobetoPlatformDbContext(builder.Options);
        }
    }
}