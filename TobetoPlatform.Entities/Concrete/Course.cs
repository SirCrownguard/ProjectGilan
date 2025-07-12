using TobetoPlatform.Entities.Abstract;
using System.Collections.Generic; // ICollection için eklendi

namespace TobetoPlatform.Entities.Concrete
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }

        // --- EKLENEN YENİ ALAN ---
        public string Description { get; set; } // Bu alan eksikti, eklendi.

        // İlişkiler
        public virtual Category Category { get; set; }
        public virtual ICollection<CourseFaq> CourseFaqs { get; set; }
    }
}