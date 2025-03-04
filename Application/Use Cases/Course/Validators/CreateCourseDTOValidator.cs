using Application.Use_Cases.Course.DTOs;
using FluentValidation;

namespace Application.Use_Cases.Course.Validators
{
    public class CreateCourseDTOValidator : AbstractValidator<CreateCourseDTO>
    {
        public CreateCourseDTOValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название курса обязательно.")
                .MaximumLength(100).WithMessage("Название курса не должно превышать 100 символов.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Описание курса обязательно.")
                .MaximumLength(500).WithMessage("Описание курса не должно превышать 500 символов.");

            RuleFor(x => x.CategoryIds)
                .NotNull().WithMessage("Список категорий не может быть пустым.")
                .When(x => x.CategoryIds != null)
                .ForEach(categoryId =>
                    categoryId.GreaterThan(0).WithMessage("ID категории должен быть больше 0."));
        }
    }
}
