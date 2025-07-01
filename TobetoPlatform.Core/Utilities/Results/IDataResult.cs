// TobetoPlatform.Core/Utilities/Results/IDataResult.cs
namespace TobetoPlatform.Core.Utilities.Results
{
    // Bu arayüz, işlem sonucunda veri de döndürmek istediğimizde kullanılır.
    // IResult'tan miras alır ve ek olarak bir 'Data' özelliği ekler.
    public interface IDataResult<T> : IResult
    {
        T Data { get; }
    }
}