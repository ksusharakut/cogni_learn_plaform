using Application.Use_Cases.AnswerOption.DTOs;
using FluentValidation;

namespace Application.Use_Cases.AnswerOption.Validators
{
    public class CreateAnswerOptionDTOValidator : AbstractValidator<CreateAnswerOptionDTO>
    {
        public CreateAnswerOptionDTOValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Текст варианта ответа обязателен.")
                .MaximumLength(255).WithMessage("Текст варианта ответа не должен превышать 255 символов.");
        }
    }
}
