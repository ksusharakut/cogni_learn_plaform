using Application.Use_Cases.Lesson.DTOs;
using FluentValidation;

namespace Application.Use_Cases.Lesson.Validators
{
    public class ReorderLessonsDTOValidator : AbstractValidator<ReorderLessonsDTO>
    {
        public ReorderLessonsDTOValidator()
        {
            RuleFor(x => x.Lessons)
                .NotNull().WithMessage("Список уроков не может быть пустым.")
                .NotEmpty().WithMessage("Список уроков не может быть пустым.")
                .Must(lessons => lessons.Select(l => l.LessonId).Distinct().Count() == lessons.Count)
                .WithMessage("Идентификаторы уроков не должны повторяться.");

            RuleForEach(x => x.Lessons)
                .ChildRules(lesson =>
                {
                    lesson.RuleFor(l => l.LessonId)
                        .GreaterThan(0).WithMessage("LessonId должен быть больше 0.");

                    lesson.RuleFor(l => l.OrderIndex)
                        .GreaterThanOrEqualTo(0).WithMessage("OrderIndex должен быть больше или равен 0.");
                });
        }
    }
}
