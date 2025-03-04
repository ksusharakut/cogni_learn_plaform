using Application.Exceptions;
using Application.Use_Cases.Course.DTOs;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Course.Manage_Courses.UpdateCourse
{
    public class UpdateCourseUseCase : IUpdateCourseUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCourseDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UpdateCourseUseCase(
            IUnitOfWork unitOfWork,
            IValidator<CreateCourseDTO> validator,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(int id, CreateCourseDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new AuthenticationFailedException("Невозможно идентифицировать текущего пользователя.");
            }

            var course = await _unitOfWork.CourseRepository.GetByIdAsync(id, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException($"Курс с идентификатором {id} не найден.");
            }

            if (course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете изменять только свои курсы.");
            }

            _mapper.Map(request, course);
            course.UpdatedAt = DateTimeOffset.UtcNow;

            if (request.CategoryIds != null)
            {
                var newCategories = await _unitOfWork.CategoryRepository.GetByIdsAsync(request.CategoryIds, cancellationToken);
                if (newCategories.Count != request.CategoryIds.Count) 
                {
                    throw new NotFoundException("Одна или более категория не найдена.");
                }

                course.Categories.Clear();
                foreach (var category in newCategories)
                {
                    course.Categories.Add(category);
                }
            }

            _unitOfWork.CourseRepository.Update(course);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
