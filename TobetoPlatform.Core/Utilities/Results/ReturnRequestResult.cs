// Konum: TobetoPlatform.Core/Utilities/Results/ReturnRequestResult.cs
namespace TobetoPlatform.Core.Utilities.Results
{
    public class ReturnRequestResult
    {
        public bool result { get; set; }
        public string message { get; set; }

        public ReturnRequestResult()
        {
            result = false;
            message = "İşlem sırasında bir hata oluştu.";
        }
    }
}