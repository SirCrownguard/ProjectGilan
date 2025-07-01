// SON VE DOĞRU HALİ: TobetoPlatform.API/Controllers/CategoriesController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TobetoPlatform.Business.Abstract;
using TobetoPlatform.Entities.DTOs; // DİKKAT: Artık DTO namespace'ini kullanıyoruz.

namespace TobetoPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // --- READ Operasyonları (Bunlar değişmedi, çünkü DTO almazlar) ---

        [HttpGet("getlist")]
        public IActionResult GetList()
        {
            var result = _categoryService.GetList();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid/{id}")]
        public IActionResult GetById(int id)
        {
            var result = _categoryService.GetById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Message);
        }

        // --- WRITE Operasyonları (Bunlar artık Request DTO'su alacak) ---

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        // DEĞİŞİKLİK: Category (Entity) yerine CategoryRequest (DTO) alıyor
        public IActionResult Add(CategoryRequest request)
        {
            var result = _categoryService.Add(request);
            if (result.Success)
            {
                return Ok(result); // Basit bir OK yanıtı yeterli
            }
            return BadRequest(result.Message); // Hata mesajını dön
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        // DEĞİŞİKLİK: Category (Entity) yerine CategoryRequest (DTO) alıyor
        public IActionResult Update(CategoryRequest request)
        {
            var result = _categoryService.Update(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        // DEĞİŞİKLİK: Artık Body'den Category (Entity) değil, CategoryRequest (DTO) alıyor
        public IActionResult Delete([FromBody] CategoryRequest request)
        {
            var result = _categoryService.Delete(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}