using Application.Use_Cases.Course.DTOs;
using Application.Use_Cases.Course.GetAllCourses;
using AutoMapper;
using Domain.Interfaces;

namespace Application.Use_Cases.Course.GetAllPublishedCourses
{
    public class GetPublishedCoursesUseCase : IGetPublishedCoursesUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPublishedCoursesUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseDTO>> ExecuteAsync(CancellationToken cancellationToken)
        {
            var courses = await _unitOfWork.CourseRepository.GetAllPublishedAsync(cancellationToken);
            return _mapper.Map<IEnumerable<CourseDTO>>(courses);
        }
    }
}
