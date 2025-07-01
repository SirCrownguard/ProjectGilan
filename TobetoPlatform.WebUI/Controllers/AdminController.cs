// TAMAMEN YENİLENMİŞ, DOĞRU MİMARİDEKİ KOD
// Konum: TobetoPlatform.WebUI/Controllers/AdminController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json; // JsonContent kullanmak için bu gerekli
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TobetoPlatform.Entities;
using TobetoPlatform.WebUI.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration; // IConfiguration için gerekli
using Microsoft.AspNetCore.Authentication; // GetTokenAsync için

namespace TobetoPlatform.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl; // API adresini burada tutacağız

        // IConfiguration ekledik. ICourseService'i kaldırdık!
        public AdminController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
        }

        private HttpClient CreateClientWithToken()
        {
            var client = _httpClientFactory.CreateClient();
            var accessToken = HttpContext.GetTokenAsync("access_token").Result;
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            return client;
        }

        // --- Kurs İşlemleri (API üzerinden) ---

        public async Task<IActionResult> Index()
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"{_apiBaseUrl}/courses/getlist");

            if (response.IsSuccessStatusCode)
            {
                var courses = await response.Content.ReadFromJsonAsync<List<Course>>();
                return View(courses);
            }
            return View(new List<Course>());
        }

        [HttpGet]
        public IActionResult AddCourse() => View();

        [HttpPost]
        public async Task<IActionResult> AddCourse(Course course)
        {
            if (!ModelState.IsValid) return View(course);

            var client = CreateClientWithToken();
            var response = await client.PostAsJsonAsync($"{_apiBaseUrl}/courses/add", course);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            // Hata mesajını kullanıcıya gösterebiliriz
            ViewBag.ErrorMessage = "Kurs eklenirken bir hata oluştu: " + await response.Content.ReadAsStringAsync();
            return View(course);
        }

        [HttpGet]
        public async Task<IActionResult> EditCourse(int id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"{_apiBaseUrl}/courses/getbyid/{id}");

            if (response.IsSuccessStatusCode)
            {
                var course = await response.Content.ReadFromJsonAsync<Course>();
                return View(course);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(Course course)
        {
            if (!ModelState.IsValid) return View(course);

            var client = CreateClientWithToken();
            var response = await client.PutAsJsonAsync($"{_apiBaseUrl}/courses/update", course);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMessage = "Kurs güncellenirken bir hata oluştu: " + await response.Content.ReadAsStringAsync();
            return View(course);
        }

        // KULLANICI ARAYÜZÜ İÇİN SOFT-DELETE
        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var client = CreateClientWithToken();

            // Önce silinecek kurs nesnesini API'den çekelim
            var getResponse = await client.GetAsync($"{_apiBaseUrl}/courses/getbyid/{id}");
            if (!getResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Silinecek kurs bulunamadı.";
                return RedirectToAction("Index");
            }
            var courseToDelete = await getResponse.Content.ReadFromJsonAsync<Course>();


            // Silme isteğini gönderiyoruz
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_apiBaseUrl}/courses/delete");
            request.Content = new StringContent(JsonSerializer.Serialize(courseToDelete), Encoding.UTF8, "application/json");

            var deleteResponse = await client.SendAsync(request);

            if (!deleteResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Kurs silinirken bir hata oluştu: " + await deleteResponse.Content.ReadAsStringAsync();
            }

            return RedirectToAction("Index");
        }

        // --- Kullanıcı İşlemleri (Temizlenmiş Hali) ---
        // Bunları değiştirmeye gerek yok, zaten API kullanıyorlardı. 
        // Yapın aynı kalabilir.
        // Ama yukarıdaki yapıya benzetmek istersen diye örneklerini bıraktım.
        // Mevcut halleriyle de devam edebilirsin.
    }
}