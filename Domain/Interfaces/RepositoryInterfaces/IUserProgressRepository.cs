using Domain.Entities;

namespace Domain.Interfaces.RepositoryInterfaces
{
    public interface IUserProgressRepository
    {
        Task AddAsync(UserProgress progress, CancellationToken cancellationToken);
        void Update(UserProgress progress);
        Task<UserProgress> GetByUserAndChapterAsync(int userId, int chapterId, CancellationToken cancellationToken);
        Task<UserProgress> GetByUserAndLessonAsync(int userId, int lessonId, CancellationToken cancellationToken);
        Task<UserProgress> GetByUserAndQuestionAsync(int userId, int questionId, CancellationToken cancellationToken);
        Task<List<UserProgress>> GetByUserAndCourseAsync(int userId, int courseId, CancellationToken cancellationToken);
    }
}
