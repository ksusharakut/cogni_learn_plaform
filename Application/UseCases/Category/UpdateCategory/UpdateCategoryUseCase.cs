using Application.Exceptions;
using Application.Use_Cases.Category.DTOs;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Use_Cases.Category.UpdateCategory
{
    public class UpdateCategoryUseCase : IUpdateCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCategoryDTO> _validator;

        public UpdateCategoryUseCase(IUnitOfWork unitOfWork, IValidator<CreateCategoryDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task ExecuteAsync(int id, CreateCategoryDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id, cancellationToken);
            if (category == null)
            {
                throw new NotFoundException($"Категория с идентификатором {id} не найдена.");
            }

            category.CategoryName = request.CategoryName;
            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
