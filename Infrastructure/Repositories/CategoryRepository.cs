using Domain.Entities;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == name, cancellationToken);
        }

        public async Task<Category> GetByIdAsync(int categoryId, CancellationToken cancellationToken)
        {
            return await _context.Categories.Include(c => c.Courses).FirstOrDefaultAsync(c => c.CategoryId == categoryId, cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Categories.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Category category, CancellationToken cancellationToken)
        {
            await _context.Categories.AddAsync(category, cancellationToken);
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task<List<Category>> GetByIdsAsync(IEnumerable<int> categoryIds, CancellationToken cancellationToken)
        {
            return await _context.Categories
                .Where(c => categoryIds.Contains(c.CategoryId))
                .ToListAsync(cancellationToken);
        }
    }
}
