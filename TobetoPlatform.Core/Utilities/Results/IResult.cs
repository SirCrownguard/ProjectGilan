// TobetoPlatform.Core/Utilities/Results/IResult.cs
namespace TobetoPlatform.Core.Utilities.Results
{
    // Bu, tüm operasyon sonuçları için temel arayüzdür.
    // Bir işlemin başarılı olup olmadığını ve bir mesaj içerip içermediğini söyler.
    public interface IResult
    {
        bool Success { get; }
        string Message { get; }
    }
}