using Application.Use_Cases.Category.CreateCategory;
using Application.Use_Cases.Category.DeleteCategory;
using Application.Use_Cases.Category.DTOs;
using Application.Use_Cases.Category.GetAllCategories;
using Application.Use_Cases.Category.GetCategory;
using Application.Use_Cases.Category.UpdateCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICreateCategoryUseCase _createCategoryUseCase;
        private readonly IDeleteCategoryUseCase _deleteCategoryUseCase;
        private readonly IGetCategoriesUseCase _getCategoriesUseCase;
        private readonly IGetCategoryUseCase _getCategoryUseCase;
        private readonly IUpdateCategoryUseCase _updateCategoryUseCase;

        public CategoryController(ICreateCategoryUseCase createCategoryUseCase,
            IDeleteCategoryUseCase deleteCategoryUseCase,
            IGetCategoriesUseCase getCategoriesUseCase,
            IGetCategoryUseCase getCategoryUseCase,
            IUpdateCategoryUseCase updateCategoryUseCase)
        {
            _createCategoryUseCase = createCategoryUseCase;
            _deleteCategoryUseCase = deleteCategoryUseCase;
            _getCategoriesUseCase = getCategoriesUseCase;
            _getCategoryUseCase = getCategoryUseCase;
            _updateCategoryUseCase = updateCategoryUseCase;
        }

        [HttpPost("create")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> CreateRole([FromBody] CreateCategoryDTO request, CancellationToken cancellationToken)
        {
            await _createCategoryUseCase.ExecuteAsync(request, cancellationToken);
            return Ok("Категория успешно создана.");
        }

        [HttpDelete("delete{categoryId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteRole(int categoryId, CancellationToken cancellationToken)
        {
            await _deleteCategoryUseCase.ExecuteAsync(categoryId, cancellationToken);
            return Ok("Категория успешно удалена");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
        {
            var categories = await _getCategoriesUseCase.ExecuteAsync(cancellationToken);
            return Ok(categories);
        }

        [HttpGet("byid/{id}")]
        public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
        {
            var category = await _getCategoryUseCase.ExecuteAsync(id, cancellationToken);
            return Ok(category);
        }

        [HttpPut("update/{id}")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> UpdateCategoryById(int id, [FromBody] CreateCategoryDTO request, CancellationToken cancellationToken)
        {
            await _updateCategoryUseCase.ExecuteAsync(id, request, cancellationToken);
            return Ok("Роль была успешно обновлена");
        }
    }
}
