// --- G�NCELLENM�� VE TAM PROGRAM.CS KODU ---

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

// Veritaban� ba�lant�s�n� ekle
var connectionString = builder.Configuration.GetConnectionString("TobetoDb");
builder.Services.AddDbContext<TobetoPlatformDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- DE����KL�K BURADA ---
// Art�k t�m servislerimizi projeye tan�t�yoruz.
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICourseFaqService, CourseFaqService>();
builder.Services.AddScoped<IAuthService, AuthService>();
// --- DE����KL�K B�TT� ---

var app = builder.Build();

// 2. HTTP Request Pipeline'� Ayarlama
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// �NEML� SIRA: �nce "kimsin" diye sor, sonra "yetkin var m�" diye kontrol et.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();