// Konum: TobetoPlatform.Business/Services/AuthService.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TobetoPlatform.Business.Abstract;
using TobetoPlatform.Core.Utilities.Results;
using TobetoPlatform.Core.Utilities.Security.Hashing;
using TobetoPlatform.Core.Utilities.Security.JWT;
using TobetoPlatform.DataAccess;
using TobetoPlatform.Entities;
using TobetoPlatform.Entities.DTOs;

namespace TobetoPlatform.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly TobetoPlatformDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(TobetoPlatformDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public List<Role> GetRoles(User user)
        {
            var result = from userRole in _context.UserRoles
                         join role in _context.Roles on userRole.RoleId equals role.Id
                         where userRole.UserId == user.Id
                         select new Role { Id = role.Id, Name = role.Name };
            return result.ToList();
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _context.Users.Any(u => u.Email == userForRegisterDto.Email);
            if (userExists)
            {
                return new DataResult<User>(null, false, "Bu e-posta adresi zaten kullanılıyor.");
            }

            HashingHelper.HazırlaPasswordHash(userForRegisterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            var userRole = _context.Roles.FirstOrDefault(r => r.Name == "User");
            if (userRole != null)
            {
                _context.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = userRole.Id });
                _context.SaveChanges();
            }

            return new DataResult<User>(user, true, "Kayıt başarılı");
        }

        public IDataResult<AccessToken> HazırlaAccessToken(User user)
        {
            var tokenOptions = _configuration.GetSection("TokenOptions");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions["SecurityKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };

            var userRoles = GetRoles(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Name));
            }

            var tokenDescriptor = new JwtSecurityToken(
                issuer: tokenOptions["Issuer"],
                audience: tokenOptions["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(tokenOptions["AccessTokenExpiration"])),
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = new AccessToken
            {
                Token = tokenHandler.WriteToken(tokenDescriptor),
                Expiration = tokenDescriptor.ValidTo
            };

            return new DataResult<AccessToken>(accessToken, true, "Token hazırlandı");
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _context.Users.FirstOrDefault(u => u.Email == userForLoginDto.Email);
            if (userToCheck == null)
            {
                return new DataResult<User>(null, false, "Kullanıcı bulunamadı.");
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new DataResult<User>(null, false, "Şifre hatalı.");
            }

            return new DataResult<User>(userToCheck, true, "Giriş başarılı");
        }
    }
}