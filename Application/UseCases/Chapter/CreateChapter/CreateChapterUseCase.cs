using Application.Exceptions;
using Application.Use_Cases.Chapter.DTOs;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Chapter.CreateChapter
{
    public class CreateChapterUseCase : ICreateChapterUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateChapterDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CreateChapterUseCase(
            IUnitOfWork unitOfWork,
            IValidator<CreateChapterDTO> validator,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<int> ExecuteAsync(int courseId, CreateChapterDTO request, CancellationToken cancellationToken)
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

            var course = await _unitOfWork.CourseRepository.GetByIdAsync(courseId, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException($"Курс с идентификатором {courseId} не найден.");
            }

            if (course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете добавлять главы только в свои курсы.");
            }

            var maxOrderIndex = await _unitOfWork.ChapterRepository.GetMaxOrderIndexAsync(courseId, cancellationToken);
            var newOrderIndex = maxOrderIndex + 1;

            var chapter = _mapper.Map<Domain.Entities.Chapter>(request);
            chapter.CourseId = courseId; 
            chapter.OrderIndex = newOrderIndex; 
            chapter.UpdatedAt = DateTimeOffset.UtcNow;

            await _unitOfWork.ChapterRepository.AddAsync(chapter, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return chapter.ChapterId;
        }
    }
}
