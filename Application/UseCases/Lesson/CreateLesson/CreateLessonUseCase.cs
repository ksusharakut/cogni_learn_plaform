using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Use_Cases.Lesson.CreateLesson
{
    public class CreateLessonUseCase : ICreateLessonUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateLessonDTO> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CreateLessonUseCase(
            IUnitOfWork unitOfWork,
            IValidator<CreateLessonDTO> validator,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<int> ExecuteAsync(int chapterId, CreateLessonDTO request, CancellationToken cancellationToken)
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

            var chapter = await _unitOfWork.ChapterRepository.GetByIdWithCourseAsync(chapterId, cancellationToken);
            if (chapter == null)
            {
                throw new NotFoundException($"Глава с идентификатором {chapterId} не найдена.");
            }

            if (chapter.Course.UserId != userId)
            {
                throw new AuthorizationException("Вы можете добавлять уроки только в свои курсы.");
            }

            var maxOrderIndex = await _unitOfWork.LessonRepository.GetMaxOrderIndexAsync(chapterId, cancellationToken);
            var newOrderIndex = maxOrderIndex + 1;

            var lesson = _mapper.Map<Domain.Entities.Lesson>(request);
            lesson.ChapterId = chapterId;
            lesson.OrderIndex = newOrderIndex;
            lesson.UpdatedAt = DateTimeOffset.UtcNow;

            await _unitOfWork.LessonRepository.AddAsync(lesson, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return lesson.LessonId;
        }
    }
}
