// GÜNCELLENMİŞ KOD: TobetoPlatform.API/Controllers/UsersController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TobetoPlatform.DataAccess;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly TobetoPlatformDbContext _context;

    public UsersController(TobetoPlatformDbContext context)
    {
        _context = context;
    }

    [HttpGet("getall")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _context.Users
            .Where(u => !u.IsDeleted) // DEĞİŞİKLİK: Silinmemiş kullanıcıları getir
            .Select(u => new
            {
                u.Id, // DEĞİŞİKLİK: UserId yerine Id
                u.FirstName,
                u.LastName,
                u.Email
            })
            .ToListAsync();

        // DEĞİŞİKLİK: Standart sonuç formatında döndürüyoruz
        return Ok(new { Success = true, Message = "Kullanıcılar listelendi.", Data = users });
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound(new { Success = false, Message = "Kullanıcı bulunamadı." });
        }

        user.IsDeleted = true; // DEĞİŞİKLİK: Hard delete yerine Soft Delete
        await _context.SaveChangesAsync();

        return Ok(new { Success = true, Message = "Kullanıcı başarıyla silindi." });
    }
}