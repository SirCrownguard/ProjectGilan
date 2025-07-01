// GÜNCELLENMİŞ HALİ: TobetoPlatform.Business/Abstract/ICourseService.cs
using System.Collections.Generic;
using TobetoPlatform.Core.Utilities.Results;
using TobetoPlatform.Entities;
using TobetoPlatform.Entities.DTOs; // Bunu ekle

namespace TobetoPlatform.Business.Abstract
{
    public interface ICourseService
    {
        IResult Add(CourseRequest request);
        IDataResult<Course> GetById(int courseId);
        IDataResult<List<Course>> GetList();
        IDataResult<List<Course>> GetListByCategory(int categoryId);
        IResult Update(CourseRequest request);
        IResult Delete(CourseRequest request);
    }
}