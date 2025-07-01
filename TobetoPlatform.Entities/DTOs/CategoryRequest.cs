// Konum: TobetoPlatform.Entities/DTOs/CategoryRequest.cs
namespace TobetoPlatform.Entities.DTOs
{
    public class CategoryRequest : BaseDto // BaseDto'dan miras al
    {
        public string Name { get; set; }

        public CategoryRequest() // Constructor
        {
            id = "0"; // Default olarak string "0" verelim
            Name = "";
        }
    }
}