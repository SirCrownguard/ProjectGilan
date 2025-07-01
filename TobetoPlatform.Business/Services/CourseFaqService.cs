// Konum: TobetoPlatform.Business/Services/CourseFaqService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using TobetoPlatform.Business.Abstract;
using TobetoPlatform.Core.Utilities.Results;
using TobetoPlatform.DataAccess;
using TobetoPlatform.Entities;
using TobetoPlatform.Entities.DTOs;

namespace TobetoPlatform.Business.Services
{
    public class CourseFaqService : ICourseFaqService
    {
        private readonly TobetoPlatformDbContext _context;

        public CourseFaqService(TobetoPlatformDbContext context)
        {
            _context = context;
        }

        public IResult Add(CourseFaqRequest request)
        {
            if (!int.TryParse(request.CourseId, out int courseId)) return new ErrorResult("Geçersiz Kurs ID.");

            var faq = new CourseFaq
            {
                CourseId = courseId,
                Question = request.Question,
                Answer = request.Answer
            };
            _context.CourseFaqs.Add(faq);
            _context.SaveChanges();
            return new SuccessResult("S.S.S. eklendi.");
        }

        public IResult Update(CourseFaqRequest request)
        {
            if (!int.TryParse(request.id, out int faqId)) return new ErrorResult("Geçersiz S.S.S. ID.");

            var faqToUpdate = _context.CourseFaqs.FirstOrDefault(f => f.Id == faqId);
            if (faqToUpdate == null) return new ErrorResult("S.S.S. bulunamadı.");

            faqToUpdate.Question = request.Question;
            faqToUpdate.Answer = request.Answer;
            _context.CourseFaqs.Update(faqToUpdate);
            _context.SaveChanges();
            return new SuccessResult("S.S.S. güncellendi.");
        }

        public IResult Delete(CourseFaqRequest request)
        {
            if (!int.TryParse(request.id, out int faqId)) return new ErrorResult("Geçersiz S.S.S. ID.");

            var faqToDelete = _context.CourseFaqs.FirstOrDefault(f => f.Id == faqId);
            if (faqToDelete == null) return new ErrorResult("S.S.S. bulunamadı.");

            faqToDelete.IsDeleted = true;
            _context.CourseFaqs.Update(faqToDelete);
            _context.SaveChanges();
            return new SuccessResult("S.S.S. silindi.");
        }

        public IDataResult<CourseFaq> GetById(int id)
        {
            return new SuccessDataResult<CourseFaq>(_context.CourseFaqs.FirstOrDefault(f => f.Id == id && !f.IsDeleted));
        }

        public IDataResult<List<CourseFaq>> GetListByCourse(int courseId)
        {
            return new SuccessDataResult<List<CourseFaq>>(_context.CourseFaqs.Where(f => f.CourseId == courseId && !f.IsDeleted).ToList());
        }
    }
}