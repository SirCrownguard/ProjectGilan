// Konum: TobetoPlatform.Business/Abstract/ICourseFaqService.cs
using System.Collections.Generic;
using TobetoPlatform.Core.Utilities.Results;
using TobetoPlatform.Entities;
using TobetoPlatform.Entities.DTOs;

namespace TobetoPlatform.Business.Abstract
{
    public interface ICourseFaqService
    {
        IResult Add(CourseFaqRequest request);
        IResult Update(CourseFaqRequest request);
        IResult Delete(CourseFaqRequest request);
        IDataResult<CourseFaq> GetById(int id);
        IDataResult<List<CourseFaq>> GetListByCourse(int courseId);
    }
}