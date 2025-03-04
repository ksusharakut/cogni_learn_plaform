using Application.Use_Cases.User.ChangePassword;
using Application.Use_Cases.User.DeleteUser;
using Application.Use_Cases.User.DTOs;
using Application.Use_Cases.User.GetAllUsers;
using Application.Use_Cases.User.GetUserProfile;
using Application.Use_Cases.User.UpdateUserProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDeleteUserUseCase _deleteUserUseCase;
        private readonly IGetAllUsersUseCase _getAllUsersUseCase;
        private readonly IGetUserProfileUseCase _getUserProfileUseCase;
        private readonly IChangePasswordUseCase _changePasswordUseCase;
        private readonly IUpdateUserProfileUseCase _updateUserProfileUseCase;

        public UserController(IDeleteUserUseCase deleteUserUseCase, 
            IGetAllUsersUseCase getAllUsersUseCase, 
            IGetUserProfileUseCase getUserProfileUseCase,
            IChangePasswordUseCase changePasswordUseCase,
            IUpdateUserProfileUseCase updateUserProfileUseCase)
        {
            _deleteUserUseCase = deleteUserUseCase;
            _getAllUsersUseCase = getAllUsersUseCase;
            _getUserProfileUseCase = getUserProfileUseCase;
            _changePasswordUseCase = changePasswordUseCase;
            _updateUserProfileUseCase = updateUserProfileUseCase;
        }

        [HttpGet("me/profile")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
        {
            var profile = await _getUserProfileUseCase.ExecuteAsync(cancellationToken);
            return Ok(profile);
        }

        [HttpGet("{userId}/profile")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetUserProfile(int userId, CancellationToken cancellationToken)
        {
            var profile = await _getUserProfileUseCase.ExecuteAsync(userId, cancellationToken);
            return Ok(profile);
        }

            [HttpDelete("delete-yourself")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> DeleteUserB(CancellationToken cancellationToken)
        {
            await _deleteUserUseCase.ExecuteAsync(cancellationToken);
            return Ok("Пользовотель успешно удалён");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteUserById(int id, CancellationToken cancellationToken)
        {
            await _deleteUserUseCase.ExecuteAsync(id, cancellationToken);
            return Ok($"Пользовотель с идентификатором {id} успешно удалён");
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")] 
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            var users = await _getAllUsersUseCase.ExecuteAsync(cancellationToken);
            return Ok(users);
        }

        [HttpPut("me/password")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> ChangeMyPassword([FromBody] ChangePasswordDTO request, CancellationToken cancellationToken)
        {
            await _changePasswordUseCase.ExecuteAsync(request, cancellationToken);
            return Ok("Пароль успешно изменён!");
        }

        [HttpPut("me/profile")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> ChangeMyProfile([FromBody] UpdateUserProfileDTO request, CancellationToken cancellationToken)
        {
            await _updateUserProfileUseCase.ExecuteAsync(request, cancellationToken);
            return Ok("Профиль успешно изменён!");
        }
    }
}
