// Konum: TobetoPlatform.Entities/DTOs/CourseRequest.cs
namespace TobetoPlatform.Entities.DTOs
{
    // Artık BaseDto'dan miras ALMIYOR.
    public class CourseRequest
    {
        // CategoryId'nin veri tipini int yaptık, çünkü bu bir kimlik numarasıdır.
        public int CategoryId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        // Price (fiyat) için en doğru tip decimal'dır. Metin (string) olarak tutulmamalıdır.
        public decimal Price { get; set; }
    }
}