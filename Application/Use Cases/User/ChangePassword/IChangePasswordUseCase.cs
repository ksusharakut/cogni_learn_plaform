using Application.Use_Cases.User.DTOs;

namespace Application.Use_Cases.User.ChangePassword
{
    public interface IChangePasswordUseCase
    {
        Task ExecuteAsync(ChangePasswordDTO request, CancellationToken cancellationToken);
    }
}
