// GÜNCELLENMİŞ HALİ: TobetoPlatform.Entities/Course.cs
using TobetoPlatform.Entities.Abstract;

namespace TobetoPlatform.Entities
{
	public class Course : BaseEntity // BaseEntity'den miras alıyor
	{
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }

		public virtual Category Category { get; set; }

		public Course() // Constructor ile default değerler
		{
			// BaseEntity'den gelen alanlar
			IsActive = true;
			IsDeleted = false;
			CreatedDate = DateTime.Now;

			// Course'a ait alanlar
			Name = string.Empty;
			Description = string.Empty;
			Price = 0;
		}
	}
}