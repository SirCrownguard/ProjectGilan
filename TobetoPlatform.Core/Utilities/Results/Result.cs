// TobetoPlatform.Core/Utilities/Results/Result.cs
namespace TobetoPlatform.Core.Utilities.Results
{
    public class Result : IResult
    {
        // 'this' anahtar kelimesi, bu sınıftaki diğer bir constructor'ı çağırmak için kullanılır.
        // Bu sayede kod tekrarını önleriz.
        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }

        public Result(bool success)
        {
            Success = success;
            Message = string.Empty; // Mesajı varsayılan olarak boş ayarlıyoruz.
        }

        public bool Success { get; }
        public string Message { get; }
    }

    // Başarılı durumlar için özel bir sınıf
    public class SuccessResult : Result
    {
        public SuccessResult(string message) : base(true, message) { }
        public SuccessResult() : base(true) { }
    }

    // Başarısız durumlar için özel bir sınıf
    public class ErrorResult : Result
    {
        public ErrorResult(string message) : base(false, message) { }
        public ErrorResult() : base(false) { }
    }
}