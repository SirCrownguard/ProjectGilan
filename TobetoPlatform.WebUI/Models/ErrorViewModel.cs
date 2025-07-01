// GÜNCELLENMÝÞ KOD: TobetoPlatform.WebUI/Models/ErrorViewModel.cs
namespace TobetoPlatform.WebUI.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}