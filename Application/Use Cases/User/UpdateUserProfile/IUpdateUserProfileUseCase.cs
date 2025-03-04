using Application.Use_Cases.User.DTOs;

namespace Application.Use_Cases.User.UpdateUserProfile
{
    public interface IUpdateUserProfileUseCase
    {
        Task ExecuteAsync(UpdateUserProfileDTO request, CancellationToken cancellationToken);
    }
}
