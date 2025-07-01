// Konum: TobetoPlatform.Entities/UserRole.cs
using TobetoPlatform.Entities.Abstract;

namespace TobetoPlatform.Entities
{
    // EKSİK OLAN KISIM: : BaseEntity
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}