using Application.Use_Cases.Chapter.DTOs;
using FluentValidation;

namespace Application.Use_Cases.Chapter.Validators
{
    public class ReorderChaptersDTOValidator : AbstractValidator<ReorderChaptersDTO>
    {
        public ReorderChaptersDTOValidator()
        {
            RuleFor(x => x.Chapters)
                .NotNull().WithMessage("Список глав не может быть пустым.")
                .NotEmpty().WithMessage("Список глав не может быть пустым.")
                .Must(chapters => chapters.Select(c => c.ChapterId).Distinct().Count() == chapters.Count)
                .WithMessage("Идентификаторы глав не должны повторяться.");

            RuleForEach(x => x.Chapters)
                .ChildRules(chapter =>
                {
                    chapter.RuleFor(c => c.ChapterId)
                        .GreaterThan(0).WithMessage("ChapterId должен быть больше 0.");

                    chapter.RuleFor(c => c.OrderIndex)
                        .GreaterThanOrEqualTo(0).WithMessage("OrderIndex должен быть больше или равен 0.");
                });
        }
    }
}
