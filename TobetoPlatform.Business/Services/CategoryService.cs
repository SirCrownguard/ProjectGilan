// GÜNCELLENMİŞ VE ARKADAŞININ İSTEDİĞİ MİMARİDE TAM KOD
// Konum: TobetoPlatform.Business/Services/CategoryService.cs

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TobetoPlatform.Business.Abstract;
using TobetoPlatform.Core.Utilities.Results;
using TobetoPlatform.DataAccess;
using TobetoPlatform.Entities;
using TobetoPlatform.Entities.DTOs; // DTO namespace'ini ekledik

namespace TobetoPlatform.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly TobetoPlatformDbContext _context;

        public CategoryService(TobetoPlatformDbContext context)
        {
            _context = context;
        }

        // --- Create ---
        // Artık Entity yerine Request DTO alıyor
        public IResult Add(CategoryRequest request)
        {
            // İş Kuralı: Kategori adı boş olamaz.
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new ErrorResult("Kategori adı boş olamaz.");
            }

            // Yeni bir Category (Entity) nesnesi oluşturuyoruz
            var category = new Category
            {
                Name = request.Name
                // Diğer default değerler (IsActive, CreatedDate vs)
                // Category sınıfının kendi constructor'ında atanıyor.
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
            return new SuccessResult("Kategori başarıyla eklendi.");
        }

        // --- Update ---
        public IResult Update(CategoryRequest request)
        {
            // DTO'dan gelen string id'yi int'e çeviriyoruz (parse).
            if (!int.TryParse(request.id, out int categoryId))
            {
                return new ErrorResult("Geçersiz Kategori ID formatı.");
            }

            var categoryToUpdate = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (categoryToUpdate == null)
            {
                return new ErrorResult("Güncellenecek kategori bulunamadı.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new ErrorResult("Kategori adı boş olamaz.");
            }

            // Gelen yeni bilgilerle entity'yi güncelliyoruz
            categoryToUpdate.Name = request.Name;
            categoryToUpdate.UpdatedDate = DateTime.Now;

            _context.Categories.Update(categoryToUpdate);
            _context.SaveChanges();
            return new SuccessResult("Kategori başarıyla güncellendi.");
        }

        // --- Delete (Soft Delete) ---
        public IResult Delete(CategoryRequest request)
        {
            if (!int.TryParse(request.id, out int categoryId))
            {
                return new ErrorResult("Geçersiz Kategori ID formatı.");
            }

            var categoryToDelete = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (categoryToDelete == null)
            {
                return new ErrorResult("Silinecek kategori bulunamadı.");
            }

            // Soft delete işlemini yapıyoruz
            categoryToDelete.IsDeleted = true;
            categoryToDelete.DeletedDate = DateTime.Now;

            _context.Categories.Update(categoryToDelete);
            _context.SaveChanges();
            return new SuccessResult("Kategori başarıyla silindi (arşivlendi).");
        }

        // --- Read Metotları (Bunlar DTO almaz, Entity döndürür) ---
        public IDataResult<Category> GetById(int categoryId)
        {
            // Sadece silinmemiş olanları getir
            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId && !c.IsDeleted);
            if (category == null)
            {
                return new ErrorDataResult<Category>("Kategori bulunamadı.");
            }
            return new SuccessDataResult<Category>(category);
        }

        public IDataResult<List<Category>> GetList()
        {
            var categories = _context.Categories.Where(c => !c.IsDeleted).ToList();
            return new SuccessDataResult<List<Category>>(categories);
        }
    }
}