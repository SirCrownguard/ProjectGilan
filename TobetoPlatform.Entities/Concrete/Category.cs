using TobetoPlatform.Entities.Abstract; // Bunu ekledik

namespace TobetoPlatform.Entities.Concrete
{
    public class Category : BaseEntity // BaseEntity'den miras aldık
    {
        public string Name { get; set; }

        // İleride Category'nin birden fazla kursu olabileceği için ilişki
        public virtual ICollection<Course> Courses { get; set; }
    }
}