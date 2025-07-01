// TobetoPlatform.Core/Utilities/Results/DataResult.cs
namespace TobetoPlatform.Core.Utilities.Results
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public DataResult(T data, bool success, string message) : base(success, message)
        {
            Data = data;
        }

        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }

        public T Data { get; }
    }

    // Başarılı veri sonuçları için özel sınıf
    public class SuccessDataResult<T> : DataResult<T>
    {
        public SuccessDataResult(T data, string message) : base(data, true, message) { }
        public SuccessDataResult(T data) : base(data, true) { }
        public SuccessDataResult(string message) : base(default(T), true, message) { }
        public SuccessDataResult() : base(default(T), true) { }
    }

    // Başarısız veri sonuçları için özel sınıf
    public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(T data, string message) : base(data, false, message) { }
        public ErrorDataResult(T data) : base(data, false) { }
        public ErrorDataResult(string message) : base(default(T), false, message) { }
        public ErrorDataResult() : base(default(T), false) { }
    }
}