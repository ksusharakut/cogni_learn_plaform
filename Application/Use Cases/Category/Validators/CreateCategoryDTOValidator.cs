using Application.Use_Cases.Category.DTOs;
using FluentValidation;

namespace Application.Use_Cases.Category.Validators
{
    public class CreateCategoryDTOValidator : AbstractValidator<CreateCategoryDTO>
    {
        public CreateCategoryDTOValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Название категории нужно указать обязательно.")
                .MaximumLength(50).WithMessage("Название категории не должно превышать 50 символов.")
                .Matches(@"^[a-zA-Zа-яА-Я0-9 ]*$").WithMessage("Имя категории может содержать только буквы, числа, и пробелы.");
        }
    }
}
