using Application.Use_Cases.Role.CreateRole;
using Application.Use_Cases.Role.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ICreateRoleUseCase _createRoleUseCase;

        public RolesController(ICreateRoleUseCase createRoleUseCase)
        {
            _createRoleUseCase = createRoleUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO roleDTO, CancellationToken cancellationToken)
        {
            await _createRoleUseCase.ExecuteAsync(roleDTO, cancellationToken);
            return Ok("Role successfully created.");
        }
    }
}
