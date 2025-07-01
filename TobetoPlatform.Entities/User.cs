// GÜNCELLENMİŞ HALİ: TobetoPlatform.Entities/User.cs
using TobetoPlatform.Entities.Abstract;
using System.Collections.Generic;

namespace TobetoPlatform.Entities
{
	public class User : BaseEntity // BaseEntity'den miras alıyor
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public byte[] PasswordSalt { get; set; }
		public byte[] PasswordHash { get; set; }

		// Bu kullanıcı hangi rollere sahip? UserRole tablosuyla ilişki kuruyoruz.
		public virtual ICollection<UserRole> UserRoles { get; set; }

		public User() // Constructor ile default değerler
		{
			// BaseEntity'den gelen alanlar
			IsActive = true; // 'Status' alanı yerine artık bunu kullanıyoruz.
			IsDeleted = false;
			CreatedDate = DateTime.Now;

			// User'a ait alanlar
			FirstName = string.Empty;
			LastName = string.Empty;
			Email = string.Empty;
			UserRoles = new HashSet<UserRole>();
		}
	}
}