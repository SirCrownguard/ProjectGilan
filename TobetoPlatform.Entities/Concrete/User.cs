using TobetoPlatform.Entities.Abstract;
using System.Collections.Generic;

namespace TobetoPlatform.Entities.Concrete
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}