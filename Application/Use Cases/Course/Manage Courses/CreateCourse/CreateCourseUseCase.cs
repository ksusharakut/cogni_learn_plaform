using Application.Exceptions;
using Application.Use_Cases.Course.DTOs;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Course.CreateCourse
{
    public class CreateCourseUseCase : ICreateCourseUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCourseDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CreateCourseUseCase(
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

        public async Task<int> ExecuteAsync(CreateCourseDTO request, CancellationToken cancellationToken)
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

            var course = _mapper.Map<Domain.Entities.Course>(request);
            course.UserId = userId;
            course.UpdatedAt = DateTimeOffset.UtcNow;
            course.IsPublished = false;

            if (request.CategoryIds != null && request.CategoryIds.Any())
            {
                var categories = await _unitOfWork.CategoryRepository.GetByIdsAsync(request.CategoryIds, cancellationToken);
                if (categories.Count != request.CategoryIds.Count)
                {
                    throw new NotFoundException("Одна или несколько указанных категорий не найдены.");
                }
                course.Categories = categories;
            }

            await _unitOfWork.CourseRepository.AddAsync(course, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return course.CourseId;
        }
    }
}
