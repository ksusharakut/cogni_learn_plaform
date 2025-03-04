namespace Application.Use_Cases.Category.DeleteCategory
{
    public interface IDeleteCategoryUseCase
    {
        Task ExecuteAsync(int categoryId, CancellationToken cancellationToken);
    }
}
