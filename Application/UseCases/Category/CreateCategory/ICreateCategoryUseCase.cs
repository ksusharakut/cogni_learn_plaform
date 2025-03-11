using Application.Use_Cases.Category.DTOs;

namespace Application.Use_Cases.Category.CreateCategory
{
    public interface ICreateCategoryUseCase
    {
        Task ExecuteAsync(CreateCategoryDTO request, CancellationToken cancellationToken);
    }
}
