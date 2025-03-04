using Domain.Interfaces.RepositoryInterfaces;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        IPasswordResetTokenRepository PasswordResetTokenRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICourseRepository CourseRepository { get; }
        IUserCourseRepository UserCourseRepository { get; }
        IChapterRepository ChapterRepository { get; }
        IRatingRepository RatingRepository { get; }
        ILessonRepository LessonRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IAnswerOptionRepository AnswerOptionRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
