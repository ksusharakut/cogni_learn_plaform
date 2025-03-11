using Application.Use_Cases.Chapter.DTOs;
using FluentValidation;

namespace Application.Use_Cases.Chapter.Validators
{
    public class CreateChapterDTOValidator : AbstractValidator<CreateChapterDTO>
    {
        public CreateChapterDTOValidator()
        {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название главы обязательно.")
                .MaximumLength(100).WithMessage("Название главы не должно превышать 100 символов.");

        }
    }
}
