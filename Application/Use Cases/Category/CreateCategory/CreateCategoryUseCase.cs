using Application.Exceptions;
using Application.Use_Cases.Category.DTOs;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Use_Cases.Category.CreateCategory
{
    public class CreateCategoryUseCase : ICreateCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCategoryDTO> _validator;
        private readonly IMapper _mapper;

        public CreateCategoryUseCase(IUnitOfWork unitOfWork, IValidator<CreateCategoryDTO> validator, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _mapper = mapper;
        }

        public async Task ExecuteAsync(CreateCategoryDTO request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingCategory = await _unitOfWork.CategoryRepository.GetByNameAsync(request.CategoryName, cancellationToken);
            if (existingCategory != null)
            {
                throw new ConflictException($"Категория с именем '{request.CategoryName}' уже существует.");
            }

            var category = _mapper.Map<Domain.Entities.Category>(request);

            await _unitOfWork.CategoryRepository.AddAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
