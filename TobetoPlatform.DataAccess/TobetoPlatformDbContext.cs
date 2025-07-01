using Microsoft.EntityFrameworkCore;
using TobetoPlatform.Entities; // Bu satır önemli!

namespace TobetoPlatform.DataAccess;

public class TobetoPlatformDbContext : DbContext
{
    public TobetoPlatformDbContext(DbContextOptions<TobetoPlatformDbContext> options) : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }         // YENİ
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<CourseFaq> CourseFaqs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Course entity'sindeki Price özelliğinin veritabanında decimal(18, 2) olarak oluşturulmasını sağlıyoruz.
        modelBuilder.Entity<Course>().Property(c => c.Price).HasColumnType("decimal(18, 2)");

        base.OnModelCreating(modelBuilder);
    }
}