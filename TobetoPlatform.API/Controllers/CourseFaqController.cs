// Konum: TobetoPlatform.API/Controllers/CourseFaqController.cs
using Microsoft.AspNetCore.Mvc;
using TobetoPlatform.Business.Abstract;
using TobetoPlatform.Entities.DTOs;

namespace TobetoPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseFaqController : ControllerBase
    {
        private readonly ICourseFaqService _courseFaqService;

        public CourseFaqController(ICourseFaqService courseFaqService)
        {
            _courseFaqService = courseFaqService;
        }

        [HttpPost("add")]
        public IActionResult Add(CourseFaqRequest request)
        {
            var result = _courseFaqService.Add(request);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("getlistbycourse/{courseId}")]
        public IActionResult GetListByCourse(int courseId)
        {
            var result = _courseFaqService.GetListByCourse(courseId);
            return result.Success ? Ok(result.Data) : BadRequest(result);
        }
    }
}