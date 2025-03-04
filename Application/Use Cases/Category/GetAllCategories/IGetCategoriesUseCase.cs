using Application.Use_Cases.Category.DTOs;

namespace Application.Use_Cases.Category.GetAllCategories
{
    public interface IGetCategoriesUseCase
    {
        Task<IEnumerable<CategoryDTO>> ExecuteAsync(CancellationToken cancellationToken);
    }
}
