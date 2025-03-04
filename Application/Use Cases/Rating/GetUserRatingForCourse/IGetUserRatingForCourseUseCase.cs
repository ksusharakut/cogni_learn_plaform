using Application.Use_Cases.Rating.DTOs;

namespace Application.Use_Cases.Rating.GetUserRatingForCourse
{
    public interface IGetUserRatingForCourseUseCase
    {
        Task<UserRatingResultDTO> ExecuteAsync(int courseId, CancellationToken cancellationToken);
    }
}
