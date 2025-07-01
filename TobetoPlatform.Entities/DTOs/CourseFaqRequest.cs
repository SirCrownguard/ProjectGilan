// Konum: TobetoPlatform.Entities/DTOs/CourseFaqRequest.cs
namespace TobetoPlatform.Entities.DTOs
{
    // DÜZELTME: Artık BaseDto'dan miras alıyor
    public class CourseFaqRequest : BaseDto
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string CourseId { get; set; }

        public CourseFaqRequest()
        {
            id = "0";
            Question = "";
            Answer = "";
            CourseId = "0";
        }
    }
}