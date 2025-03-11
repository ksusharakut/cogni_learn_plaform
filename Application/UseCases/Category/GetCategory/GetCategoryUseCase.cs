using Application.Exceptions;
using Application.Use_Cases.Category.DTOs;
using AutoMapper;
using Domain.Interfaces;

namespace Application.Use_Cases.Category.GetCategory
{
    public class GetCategoryUseCase : IGetCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoryUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> ExecuteAsync(int categoryId, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId, cancellationToken);
            if (category == null)
            {
                throw new NotFoundException($"Category with ID {categoryId} not found.");
            }

            return _mapper.Map<CategoryDTO>(category);
        }
    }
}
