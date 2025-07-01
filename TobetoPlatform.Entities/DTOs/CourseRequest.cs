// Konum: TobetoPlatform.Entities/DTOs/CourseRequest.cs
namespace TobetoPlatform.Entities.DTOs
{
    public class CourseRequest : BaseDto // Ortak 'id' için BaseDto'dan miras
    {
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }

        public CourseRequest()
        {
            id = "0";
            CategoryId = "0";
            Name = "";
            Description = "";
            Price = "0";
        }
    }
}