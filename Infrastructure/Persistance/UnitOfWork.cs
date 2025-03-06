using Domain.Interfaces;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance.Data;

namespace Infrastructure.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRoleRepository RoleRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IPasswordResetTokenRepository PasswordResetTokenRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public ICourseRepository CourseRepository { get; set; }
        public IUserCourseRepository UserCourseRepository { get; set; }
        public IChapterRepository ChapterRepository { get; set; }
        public IRatingRepository RatingRepository { get; set; }
        public ILessonRepository LessonRepository { get; set; }
        public IQuestionRepository QuestionRepository { get; set; }
        public IAnswerOptionRepository AnswerOptionRepository { get; set; }
        public IUserProgressRepository UserProgressRepository { get; set; }
        public UnitOfWork(ApplicationDbContext context,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IPasswordResetTokenRepository passwordResetTokenRepository,
            ICategoryRepository categoryRepository,
            ICourseRepository courseRepository,
            IUserCourseRepository userCourseRepository,
            IChapterRepository chapterRepository,
            IRatingRepository ratingRepository,
            ILessonRepository lessonRepository,
            IQuestionRepository questionRepository,
            IAnswerOptionRepository answerOptionRepository,
            IUserProgressRepository userProgressRepository)
        {
            _context = context;
            RoleRepository = roleRepository;
            UserRepository = userRepository;
            PasswordResetTokenRepository = passwordResetTokenRepository;
            CategoryRepository = categoryRepository;
            CourseRepository = courseRepository;
            UserCourseRepository = userCourseRepository;
            ChapterRepository = chapterRepository;
            RatingRepository = ratingRepository;
            LessonRepository = lessonRepository;
            QuestionRepository = questionRepository;
            AnswerOptionRepository = answerOptionRepository;
            UserProgressRepository = userProgressRepository;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
