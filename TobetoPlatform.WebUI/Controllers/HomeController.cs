// G�NCELLENM�� KOD: TobetoPlatform.WebUI/Controllers/HomeController.cs

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TobetoPlatform.Business.Abstract; // DE����KL�K: ICourseService i�in
using TobetoPlatform.Entities;
using TobetoPlatform.WebUI.Models;

namespace TobetoPlatform.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService; // DE����KL�K

        public HomeController(ICourseService courseService) // DE����KL�K
        {
            _courseService = courseService;
        }

        public IActionResult Index() // DE����KL�K: async/Task kald�r�ld�
        {
            var result = _courseService.GetList(); // DE����KL�K: Yeni metot ad�
            // Ana sayfada sadece ba�ar�l� olursa kurslar� g�sterelim.
            if (result.Success)
            {
                return View(result.Data); // DE����KL�K: Sonucun i�indeki veriyi g�nderiyoruz
            }
            // Hata durumunda bo� bir liste g�nderelim
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