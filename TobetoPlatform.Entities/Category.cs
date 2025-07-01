// Konum: TobetoPlatform.Entities/Category.cs
using System.Collections.Generic;
using TobetoPlatform.Entities.Abstract; // Bunu ekle

namespace TobetoPlatform.Entities
{
    public class Category : BaseEntity // BaseEntity'den miras al
    {
        public string Name { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public Category() // Constructor ile default değerler
        {
            IsActive = true;
            IsDeleted = false;
            CreatedDate = DateTime.Now;
            Courses = new HashSet<Course>();
        }
    }
}