using Application.Use_Cases.Category.DTOs;

namespace Application.Use_Cases.Category.GetCategory
{
    public interface IGetCategoryUseCase
    {
        Task<CategoryDTO> ExecuteAsync(int categoryId, CancellationToken cancellationToken);
    }
}
