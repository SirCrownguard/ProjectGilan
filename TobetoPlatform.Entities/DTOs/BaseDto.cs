// Konum: TobetoPlatform.Entities/DTOs/BaseDto.cs
namespace TobetoPlatform.Entities.DTOs
{
    public abstract class BaseDto
    {
        // İsteklerdeki (Request) Id her zaman string gelecek.
        public string id { get; set; }
    }
}