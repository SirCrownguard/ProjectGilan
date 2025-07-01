// SON VE DOĞRU HALİ: TobetoPlatform.API/Controllers/CoursesController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TobetoPlatform.Business.Abstract;
using TobetoPlatform.Entities.DTOs; // DİKKAT: Artık DTO namespace'ini kullanıyoruz.

namespace TobetoPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // --- READ Operasyonları (Değişiklik yok) ---

        [HttpGet("getlist")]
        public IActionResult GetList()
        {
            var result = _courseService.GetList();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        [HttpGet("getlistbycategory/{categoryId}")]
        public IActionResult GetListByCategory(int categoryId)
        {
            var result = _courseService.GetListByCategory(categoryId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetById(int id)
        {
            var result = _courseService.GetById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Message);
        }

        // --- WRITE Operasyonları (Bunlar artık Request DTO'su alacak) ---

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        // DEĞİŞİKLİK: Course (Entity) yerine CourseRequest (DTO) alıyor
        public IActionResult Add(CourseRequest request)
        {
            var result = _courseService.Add(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        // DEĞİŞİKLİK: Course (Entity) yerine CourseRequest (DTO) alıyor
        public IActionResult Update(CourseRequest request)
        {
            var result = _courseService.Update(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        // DEĞİŞİKLİK: Artık Body'den Course (Entity) değil, CourseRequest (DTO) alıyor
        public IActionResult Delete([FromBody] CourseRequest request)
        {
            var result = _courseService.Delete(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}