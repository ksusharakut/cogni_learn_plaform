using Application.Use_Cases.Auth.DTOs;
using Application.Use_Cases.Auth.ForgotPassword;
using Application.Use_Cases.Auth.LogIn;
using Application.Use_Cases.Auth.LogOut;
using Application.Use_Cases.Auth.RefreshToken;
using Application.Use_Cases.Auth.Register;
using Application.Use_Cases.Auth.SetNewPassword;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRegisterUseCase _registerUseCase;
        private readonly ILogInUseCase _logInUseCase;
        private readonly IRefreshTokenUseCase _refreshTokenUseCase;
        private readonly ILogOutUseCase _logOutUseCase;
        private readonly IForgotPasswordUseCase _forgotPasswordUseCase;
        private readonly ISetNewPasswordUseCase _setNewPasswordUseCase;

        public AuthController(IRegisterUseCase registerUseCase, 
            ILogInUseCase logInUseCase,
            IRefreshTokenUseCase refreshTokenUseCase,
            ILogOutUseCase logOutUseCase,
            IForgotPasswordUseCase forgotPasswordUseCase,
            ISetNewPasswordUseCase setNewPasswordUseCase)
        {
            _registerUseCase = registerUseCase;
            _logInUseCase = logInUseCase;
            _refreshTokenUseCase = refreshTokenUseCase;
            _logOutUseCase = logOutUseCase;
            _forgotPasswordUseCase = forgotPasswordUseCase;
            _setNewPasswordUseCase = setNewPasswordUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO request, CancellationToken cancellationToken)
        {
            await _registerUseCase.ExecuteAsync(request, CancellationToken.None);
            return Ok("Регистрация прошла успешно!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn(LogInUserDTO loginDto, CancellationToken cancellationToken)
        {
            var result = await _logInUseCase.ExecuteAsync(loginDto, cancellationToken);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _refreshTokenUseCase.ExecuteAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            await _logOutUseCase.ExecuteAsync(request, cancellationToken);
            return NoContent();
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailDTO request, CancellationToken cancellationToken)
        {
            await _forgotPasswordUseCase.ExecuteAsync(request, cancellationToken);
            return Ok("Код восстановления был успещно отправлен на почту");
        }

        [HttpPost("set-new-password")]
        public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordRequest request, CancellationToken cancellationToken)
        {
            await _setNewPasswordUseCase.ExecuteAsync(request, cancellationToken);
            return Ok("Пароль успешно обновлён");
        }
    }
}
