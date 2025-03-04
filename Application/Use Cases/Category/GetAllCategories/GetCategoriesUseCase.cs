using Application.Use_Cases.Category.DTOs;
using AutoMapper;
using Domain.Interfaces;

namespace Application.Use_Cases.Category.GetAllCategories
{
    public class GetCategoriesUseCase : IGetCategoriesUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoriesUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> ExecuteAsync(CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }
    }
}
