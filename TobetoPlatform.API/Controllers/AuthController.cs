// --- BU KESİNLİKLE DOĞRU OLAN KODDUR ---

using Microsoft.AspNetCore.Mvc;
using TobetoPlatform.Business.Abstract; // Doğru namespace: Abstract
using TobetoPlatform.Entities.DTOs;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService; // IAuthService kullanıyoruz

    public AuthController(IAuthService authService) // IAuthService alıyoruz
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login(UserForLoginDto userForLoginDto)
    {
        var userToLogin = _authService.Login(userForLoginDto);
        if (!userToLogin.Success)
        {
            // userToLogin.Message'i döndürmek daha doğru
            return BadRequest(userToLogin.Message);
        }

        var result = _authService.HazırlaAccessToken(userToLogin.Data);
        if (result.Success)
        {
            // result.Data (yani AccessToken) nesnesini döndürüyoruz
            return Ok(result.Data);
        }

        return BadRequest(result.Message);
    }

    [HttpPost("register")]
    public IActionResult Register(UserForRegisterDto userForRegisterDto)
    {
        var registerResult = _authService.Register(userForRegisterDto);
        if (!registerResult.Success)
        {
            return BadRequest(registerResult.Message);
        }

        // Kayıt sonrası kullanıcıya token'ını veriyoruz
        var tokenResult = _authService.HazırlaAccessToken(registerResult.Data);
        if (tokenResult.Success)
        {
            return Ok(tokenResult.Data);
        }

        return BadRequest(tokenResult.Message);
    }
}