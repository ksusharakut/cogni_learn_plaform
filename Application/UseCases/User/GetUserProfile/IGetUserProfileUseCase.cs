using Application.Use_Cases.User.UpdateUserProfile;

namespace Application.Use_Cases.User.GetUserProfile
{
    public interface IGetUserProfileUseCase
    {
        Task<UserProfileDTO> ExecuteAsync(CancellationToken cancellationToken); 
        Task<UserProfileDTO> ExecuteAsync(int userId, CancellationToken cancellationToken); 
    }
}
