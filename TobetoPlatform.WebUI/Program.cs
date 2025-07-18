// --- G�NCELLENM�� VE TAM KOD: TobetoPlatform.WebUI/Program.cs ---

// YEN� using ifadeleri: Art�k s�zle�meleri de tan�yoruz
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TobetoPlatform.Business.Abstract; // YEN�
using TobetoPlatform.Business.Services;
using TobetoPlatform.DataAccess;
using TobetoPlatform.WebUI.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Gerekli Servisleri Ekleme
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

// 2. Veritaban� ve Kendi Servislerimizi Ekleme
var connectionString = builder.Configuration.GetConnectionString("TobetoDb");
builder.Services.AddDbContext<TobetoPlatformDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- DE����KL�K BURADA ---
// Servislerimizi "Interface => S�n�f" e�le�mesiyle kaydediyoruz.
// Bu, projenin esnek ve test edilebilir olmas�n� sa�lar.
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
// --- DE����KL�K B�TT� ---

// 3. Kimlik Do�rulama (Authentication) Ayarlar�
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Veritaban� ba�lang�� verilerini ekleme (Bu k�s�m ayn� kal�yor)
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
            logger.LogError(ex, "Veritaban� i�in ba�lang�� ayarlar� yap�l�rken bir hata olu�tu.");
        }
    }
}
await VeritabaniBaslangicAyarlariniYap(app);

// 4. HTTP �stek Ak���n� (Pipeline) Yap�land�rma
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

// 5. URL Rotalar�n� Belirleme
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 6. Uygulamay� �al��t�rma
app.Run();