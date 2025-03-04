using FluentValidation;

namespace Application.Use_Cases.Lesson.Validators
{
    public class CreateLessonDTOValidator : AbstractValidator<CreateLessonDTO>
    {
        public CreateLessonDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название урока обязательно.")
                .MaximumLength(100).WithMessage("Название урока не должно превышать 100 символов.");

            RuleFor(x => x.ContentPath)
                .NotEmpty().WithMessage("Путь к содержимому обязателен.")
                .MaximumLength(255).WithMessage("Путь к содержимому не должен превышать 255 символов.");

            RuleFor(x => x.LessonType)
                .IsInEnum().WithMessage("Неверный тип урока.");
        }
    }
}
