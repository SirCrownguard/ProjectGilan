// Konum: TobetoPlatform.Entities/Instructor.cs
using TobetoPlatform.Entities.Abstract;

namespace TobetoPlatform.Entities
{
    // EKSİK OLAN KISIM: : BaseEntity
    public class Instructor : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}