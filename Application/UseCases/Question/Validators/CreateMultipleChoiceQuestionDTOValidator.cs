using Application.Use_Cases.Question.DTOs;
using FluentValidation;

namespace Application.Use_Cases.Question.Validators
{
    public class CreateMultipleChoiceQuestionDTOValidator : AbstractValidator<CreateMultipleChoiceQuestionDTO>
    {
        public CreateMultipleChoiceQuestionDTOValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Текст вопроса обязателен.")
                .MaximumLength(500).WithMessage("Текст вопроса не должен превышать 500 символов.");

            RuleFor(x => x.AnswerOptions)
                .NotEmpty().WithMessage("Для вопросов с множественным выбором необходимо указать варианты ответа.")
                .Must(options => options.Any(a => a.IsCorrect)).WithMessage("Должен быть хотя бы один правильный вариант ответа.");

            RuleForEach(x => x.AnswerOptions)
                .ChildRules(option =>
                {
                    option.RuleFor(a => a.Text)
                        .NotEmpty().WithMessage("Текст варианта ответа обязателен.")
                        .MaximumLength(255).WithMessage("Текст варианта ответа не должен превышать 255 символов.");
                });
        }
    }
}
