// GÜNCELLENMİŞ HALİ: TobetoPlatform.Business/Abstract/ICategoryService.cs
using System.Collections.Generic;
using TobetoPlatform.Core.Utilities.Results;
using TobetoPlatform.Entities;
using TobetoPlatform.Entities.DTOs; // Bunu ekle

namespace TobetoPlatform.Business.Abstract
{
    public interface ICategoryService
    {
        // CRUD (Create-Read-Update-Delete) metotları
        IResult Add(CategoryRequest request);    // Request DTO'su alır
        IDataResult<Category> GetById(int categoryId); // Entity döner
        IDataResult<List<Category>> GetList();     // Entity listesi döner
        IResult Update(CategoryRequest request);  // Request DTO'su alır
        IResult Delete(CategoryRequest request);  // Request DTO'su alır (Soft Delete)
    }
}