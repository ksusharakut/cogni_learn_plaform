using Application.Use_Cases.Auth.DTOs;

namespace Application.Use_Cases.Auth.Register
{
    public interface IRegisterUseCase
    {
        Task ExecuteAsync(UserRegisterDTO request, CancellationToken cancellationToken);
    }
}
