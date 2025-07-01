// SON VE %100 DOĞRU HALİ
// Konum: TobetoPlatform.Entities/Role.cs

using TobetoPlatform.Entities.Abstract; // Bunu ekliyoruz

namespace TobetoPlatform.Entities
{
    // EKSİK OLAN KISIM BURASIYDI: ": BaseEntity"
    public class Role : BaseEntity
    {
        public string Name { get; set; }
    }
}