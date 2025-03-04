using Application.Use_Cases.Category.DTOs;

namespace Application.Use_Cases.Category.UpdateCategory
{
    public interface IUpdateCategoryUseCase
    {
        Task ExecuteAsync(int id, CreateCategoryDTO request, CancellationToken cancellationToken);
    }
}
