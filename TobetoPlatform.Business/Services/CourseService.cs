// GÜNCELLENMİŞ VE ARKADAŞININ İSTEDİĞİ MİMARİDE TAM KOD
// Konum: TobetoPlatform.Business/Services/CourseService.cs

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
    public class CourseService : ICourseService
    {
        private readonly TobetoPlatformDbContext _context;

        public CourseService(TobetoPlatformDbContext context)
        {
            _context = context;
        }

        // --- Create ---
        public IResult Add(CourseRequest request)
        {
            // --- Parse Etme (Çevirme) İşlemleri ---
            if (!int.TryParse(request.CategoryId, out int categoryId) || categoryId == 0)
            {
                return new ErrorResult("Geçersiz Kategori ID'si.");
            }
            if (!decimal.TryParse(request.Price, out decimal price))
            {
                return new ErrorResult("Geçersiz Fiyat formatı.");
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return new ErrorResult("Kurs adı boş olamaz.");
            }

            // --- Kurallar ---
            var categoryExists = _context.Categories.Any(c => c.Id == categoryId && !c.IsDeleted);
            if (!categoryExists)
            {
                return new ErrorResult("Böyle bir kategori bulunamadı.");
            }

            // --- Entity Oluşturma ---
            var course = new Course
            {
                CategoryId = categoryId,
                Name = request.Name,
                Description = request.Description,
                Price = price
            };

            _context.Courses.Add(course);
            _context.SaveChanges();
            return new SuccessResult("Kurs başarıyla eklendi.");
        }

        // --- Update ---
        public IResult Update(CourseRequest request)
        {
            // --- Parse Etme ---
            if (!int.TryParse(request.id, out int courseId)) return new ErrorResult("Geçersiz Kurs ID formatı.");
            if (!int.TryParse(request.CategoryId, out int categoryId)) return new ErrorResult("Geçersiz Kategori ID formatı.");
            if (!decimal.TryParse(request.Price, out decimal price)) return new ErrorResult("Geçersiz Fiyat formatı.");

            // --- Varlığı Bulma ---
            var courseToUpdate = _context.Courses.FirstOrDefault(c => c.Id == courseId);
            if (courseToUpdate == null)
            {
                return new ErrorResult("Güncellenecek kurs bulunamadı.");
            }

            // --- Kurallar ---
            if (string.IsNullOrWhiteSpace(request.Name)) return new ErrorResult("Kurs adı boş olamaz.");

            // --- Entity Güncelleme ---
            courseToUpdate.CategoryId = categoryId;
            courseToUpdate.Name = request.Name;
            courseToUpdate.Description = request.Description;
            courseToUpdate.Price = price;
            courseToUpdate.UpdatedDate = DateTime.Now;

            _context.Courses.Update(courseToUpdate);
            _context.SaveChanges();
            return new SuccessResult("Kurs başarıyla güncellendi.");
        }

        // --- Delete (Soft Delete) ---
        public IResult Delete(CourseRequest request)
        {
            if (!int.TryParse(request.id, out int courseId))
            {
                return new ErrorResult("Geçersiz Kurs ID formatı.");
            }

            var courseToDelete = _context.Courses.FirstOrDefault(c => c.Id == courseId);
            if (courseToDelete == null)
            {
                return new ErrorResult("Silinecek kurs bulunamadı.");
            }

            courseToDelete.IsDeleted = true;
            courseToDelete.DeletedDate = DateTime.Now;

            _context.Courses.Update(courseToDelete);
            _context.SaveChanges();
            return new SuccessResult("Kurs başarıyla silindi (arşivlendi).");
        }

        // --- Read Metotları (Bunlar değişmez, çünkü DTO almazlar) ---
        public IDataResult<Course> GetById(int courseId)
        {
            var course = _context.Courses.Include(c => c.Category).FirstOrDefault(c => c.Id == courseId && !c.IsDeleted);
            if (course == null)
            {
                return new ErrorDataResult<Course>("Kurs bulunamadı.");
            }
            return new SuccessDataResult<Course>(course);
        }

        public IDataResult<List<Course>> GetList()
        {
            // Include ile her kursun kategori bilgisini de yanında getiriyoruz.
            var courses = _context.Courses.Include(c => c.Category).Where(c => !c.IsDeleted).ToList();
            return new SuccessDataResult<List<Course>>(courses);
        }

        public IDataResult<List<Course>> GetListByCategory(int categoryId)
        {
            var courses = _context.Courses.Include(c => c.Category).Where(c => c.CategoryId == categoryId && !c.IsDeleted).ToList();
            return new SuccessDataResult<List<Course>>(courses);
        }
    }
}