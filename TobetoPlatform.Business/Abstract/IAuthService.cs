// Konum: TobetoPlatform.Business/Abstract/IAuthService.cs
using TobetoPlatform.Core.Utilities.Results;
using TobetoPlatform.Core.Utilities.Security.JWT;
using TobetoPlatform.Entities;
using TobetoPlatform.Entities.DTOs;

namespace TobetoPlatform.Business.Abstract
{
    public interface IAuthService
    {
        // Bu servis DTO'lar ile çalışıyor, yapısı doğru. Değişiklik gerekmiyor.
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto);
        IDataResult<User> Login(UserForLoginDto userForLoginDto);
        IDataResult<AccessToken> HazırlaAccessToken(User user);
    }
}