// GÜNCELLENMÝÞ KOD: TobetoPlatform.WebUI/Controllers/HomeController.cs

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TobetoPlatform.Business.Abstract; // DEÐÝÞÝKLÝK: ICourseService için
using TobetoPlatform.Entities;
using TobetoPlatform.WebUI.Models;

namespace TobetoPlatform.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService; // DEÐÝÞÝKLÝK

        public HomeController(ICourseService courseService) // DEÐÝÞÝKLÝK
        {
            _courseService = courseService;
        }

        public IActionResult Index() // DEÐÝÞÝKLÝK: async/Task kaldýrýldý
        {
            var result = _courseService.GetList(); // DEÐÝÞÝKLÝK: Yeni metot adý
            // Ana sayfada sadece baþarýlý olursa kurslarý gösterelim.
            if (result.Success)
            {
                return View(result.Data); // DEÐÝÞÝKLÝK: Sonucun içindeki veriyi gönderiyoruz
            }
            // Hata durumunda boþ bir liste gönderelim
            return View(new List<Course>());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}