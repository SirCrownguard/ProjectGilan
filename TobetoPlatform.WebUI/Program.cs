// --- GÜNCELLENMÝÞ VE TAM KOD: TobetoPlatform.WebUI/Program.cs ---

// YENÝ using ifadeleri: Artýk sözleþmeleri de tanýyoruz
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TobetoPlatform.Business.Abstract; // YENÝ
using TobetoPlatform.Business.Services;
using TobetoPlatform.DataAccess;
using TobetoPlatform.WebUI.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Gerekli Servisleri Ekleme
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

// 2. Veritabaný ve Kendi Servislerimizi Ekleme
var connectionString = builder.Configuration.GetConnectionString("TobetoDb");
builder.Services.AddDbContext<TobetoPlatformDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- DEÐÝÞÝKLÝK BURADA ---
// Servislerimizi "Interface => Sýnýf" eþleþmesiyle kaydediyoruz.
// Bu, projenin esnek ve test edilebilir olmasýný saðlar.
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
// --- DEÐÝÞÝKLÝK BÝTTÝ ---

// 3. Kimlik Doðrulama (Authentication) Ayarlarý
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Veritabaný baþlangýç verilerini ekleme (Bu kýsým ayný kalýyor)
async Task VeritabaniBaslangicAyarlariniYap(IApplicationBuilder uygulama)
{
    using (var kapsam = uygulama.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
    {
        var hizmetler = kapsam.ServiceProvider;
        try
        {
            await SeedDatabase.Initialize(hizmetler);
        }
        catch (Exception ex)
        {
            var logger = hizmetler.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Veritabaný için baþlangýç ayarlarý yapýlýrken bir hata oluþtu.");
        }
    }
}
await VeritabaniBaslangicAyarlariniYap(app);

// 4. HTTP Ýstek Akýþýný (Pipeline) Yapýlandýrma
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 5. URL Rotalarýný Belirleme
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 6. Uygulamayý Çalýþtýrma
app.Run();