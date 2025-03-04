using Application.Use_Cases.Role.AssignRole;
using Application.Use_Cases.Role.ChangeRole;
using Application.Use_Cases.Role.CreateRole;
using Application.Use_Cases.Role.DTOs;
using Application.Use_Cases.Role.GetRoles;
using Application.Use_Cases.Role.GetUserRoles;
using Application.Use_Cases.Role.RemoveRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace WebAPI.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ICreateRoleUseCase _createRoleUseCase;
        private readonly IGetRolesUseCase _getRolesUseCase;
        private readonly IGetUserRolesUseCase _getUserRoles;
        private readonly IAssignRoleUseCase _assignRoleUseCase;
        private readonly IRemoveRoleUseCase _removeRoleUseCase;
        private readonly IUpdateRoleUseCase _updateRoleUseCase;

        public RolesController(ICreateRoleUseCase createRoleUseCase, 
            IGetRolesUseCase getRolesUseCase,
            IGetUserRolesUseCase getUserRoles,
            IAssignRoleUseCase assignRoleUseCase,
            IRemoveRoleUseCase removeRoleUseCase,
            IUpdateRoleUseCase updateRoleUseCase)
        {
            _createRoleUseCase = createRoleUseCase;
            _getRolesUseCase = getRolesUseCase;
            _getUserRoles = getUserRoles;
            _assignRoleUseCase = assignRoleUseCase;
            _removeRoleUseCase = removeRoleUseCase;
            _updateRoleUseCase = updateRoleUseCase;
        }

        [HttpPost("create")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO roleDTO, CancellationToken cancellationToken)
        {
            await _createRoleUseCase.ExecuteAsync(roleDTO, cancellationToken);
            return Ok("Роль успешно создана.");
        }

        [HttpGet("all")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
        {
            var roles = await _getRolesUseCase.ExecuteAsync(cancellationToken);
            return Ok(roles);
        }

        [HttpGet("get-user-roles")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUserRoles(int roleId, CancellationToken cancellationToken)
        {
            var roles = await _getUserRoles.ExecuteAsync(roleId, cancellationToken);
            return Ok(roles);
        }

        [HttpPost("assign")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDTO assignRoleDTO, CancellationToken cancellationToken)
        {
            await _assignRoleUseCase.ExecuteAsync(assignRoleDTO.UserId, assignRoleDTO.RoleId, cancellationToken);
            return Ok("Роль успешно назначена");
        }

        [HttpDelete("remove")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> RemoveRole([FromBody] AssignRoleDTO removeRoleDTO, CancellationToken cancellationToken)
        {
            await _removeRoleUseCase.ExecuteAsync(removeRoleDTO.UserId, removeRoleDTO.RoleId, cancellationToken);
            return Ok("Роль была успешно убрана");
        }

        [HttpPut("update/{roleId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateRole(int roleId, RoleDTO roleDTO, CancellationToken cancellationToken)
        {
            await _updateRoleUseCase.ExecuteAsync(roleId, roleDTO, cancellationToken);
            return Ok("Роль была успешно обновлена");
        }
    }
}
