// --- GÜNCELLENMÝÞ VE TAM PROGRAM.CS KODU ---

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TobetoPlatform.Business.Abstract;
using TobetoPlatform.Business.Services;
using TobetoPlatform.DataAccess;

var builder = WebApplication.CreateBuilder(args);

var tokenOptions = builder.Configuration.GetSection("TokenOptions");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = tokenOptions["Issuer"],
            ValidAudience = tokenOptions["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions["SecurityKey"]!))
        };
    });

// 1. Servisleri Konteynera Ekleme (Dependency Injection)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Veritabaný baðlantýsýný ekle
var connectionString = builder.Configuration.GetConnectionString("TobetoDb");
builder.Services.AddDbContext<TobetoPlatformDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- DEÐÝÞÝKLÝK BURADA ---
// Artýk tüm servislerimizi projeye tanýtýyoruz.
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICourseFaqService, CourseFaqService>();
builder.Services.AddScoped<IAuthService, AuthService>();
// --- DEÐÝÞÝKLÝK BÝTTÝ ---

var app = builder.Build();

// 2. HTTP Request Pipeline'ý Ayarlama
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ÖNEMLÝ SIRA: Önce "kimsin" diye sor, sonra "yetkin var mý" diye kontrol et.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();