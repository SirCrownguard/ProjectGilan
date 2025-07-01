// Konum: TobetoPlatform.Entities/CourseFaq.cs
using TobetoPlatform.Entities.Abstract;

namespace TobetoPlatform.Entities
{
    public class CourseFaq : BaseEntity
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public int CourseId { get; set; } // Hangi kursa ait olduğunu belirtir

        public virtual Course Course { get; set; } // Navigation property

        public CourseFaq()
        {
            IsActive = true;
            IsDeleted = false;
            CreatedDate = DateTime.Now;
            Question = "";
            Answer = "";
        }
    }
}