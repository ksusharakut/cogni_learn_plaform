using Application.Exceptions;
using Domain.Interfaces;

namespace Application.Use_Cases.Category.DeleteCategory
{
    public class DeleteCategoryUseCase : IDeleteCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int categoryId, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId, cancellationToken);

            if (category == null)
            {
                throw new NotFoundException($"Категория с идентефекатором {categoryId} не найдена.");
            }

            if (category.Courses.Any())
            {
                throw new ConflictException("Не можем удалить категорию, потому что она привязана к курсу.");
            }

            _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
