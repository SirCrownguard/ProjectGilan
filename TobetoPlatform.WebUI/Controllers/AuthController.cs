// --- WebUI/Controllers/AuthController.cs dosyasının doğru hali ---

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TobetoPlatform.Entities.DTOs;

namespace TobetoPlatform.WebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto, string returnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(userForLoginDto);
            }

            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(userForLoginDto), Encoding.UTF8, "application/json");

            // API adresinin doğru olduğundan emin ol (launchSettings.json dosyasından kontrol edilebilir)
            var response = await client.PostAsync("https://localhost:7171/api/Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();

                // API'den gelen token'ı yakalamak için geçici bir model
                var tokenModel = JsonSerializer.Deserialize<JwtTokenModel>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (tokenModel != null && !string.IsNullOrEmpty(tokenModel.Token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtSecurityToken = handler.ReadJwtToken(tokenModel.Token);

                    // Token içindeki tüm bilgileri (claim'leri) alıyoruz
                    var claimsFromToken = jwtSecurityToken.Claims;

                    // Bu bilgilerle tarayıcıda saklanacak olan kimliği (cookie) oluşturuyoruz.
                    var claimsIdentity = new ClaimsIdentity(claimsFromToken, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties { IsPersistent = true }; // "Beni Hatırla" özelliği

                    // Kullanıcıyı siteye giriş yaptırıyoruz
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return LocalRedirect(returnUrl);
                }
            }

            // Eğer API'den hata dönerse veya token alınamazsa bu mesajı göster
            ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
            return View(userForLoginDto);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }

    // API'den gelen { "token": "...", "expiration": "..." } yapısını karşılamak için
    public class JwtTokenModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}