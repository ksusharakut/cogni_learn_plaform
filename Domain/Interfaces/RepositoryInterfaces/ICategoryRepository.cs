using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int categoryId, CancellationToken cancellationToken);
        Task<Category> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task<List<Category>> GetByIdsAsync(IEnumerable<int> categoryIds, CancellationToken cancellationToken);
        Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Category category, CancellationToken cancellationToken);
        void Update(Category category);
        void Delete(Category category);
    }
}
