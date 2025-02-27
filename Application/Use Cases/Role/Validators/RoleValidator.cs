using Application.Use_Cases.Role.DTOs;
using FluentValidation;

namespace Application.Use_Cases.Role.Validators
{
    public class RoleValidator : AbstractValidator<RoleDTO>
    {
        public RoleValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("RoleName is required.")
                .MaximumLength(30).WithMessage("RoleName cannot exceed 30 characters.");
        } 
    }
}
