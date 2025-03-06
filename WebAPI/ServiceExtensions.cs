using Application.Use_Cases.Role.CreateRole;
using Domain.Interfaces;
using FluentValidation;
using Domain.Interfaces.RepositoryInterfaces;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Data;
using Infrastructure.Repositories;
using Infrastructure.AutoMapper;
using Microsoft.EntityFrameworkCore;
using Application.Use_Cases.Role.Validators;
using Application.Common;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Application.Use_Cases.Auth.Register;
using Application.Use_Cases.Auth.LogIn;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Use_Cases.Auth.RefreshToken;
using Application.Use_Cases.Auth.LogOut;
using Application.Use_Cases.Auth.ForgotPassword;
using Application.Use_Cases.Auth.SetNewPassword;
using Application.Use_Cases.Role.GetRoles;
using Application.Use_Cases.Role.GetUserRoles;
using Application.Use_Cases.Role.AssignRole;
using Application.Use_Cases.Role.RemoveRole;
using Application.Use_Cases.Role.ChangeRole;
using Application.Use_Cases.Category.CreateCategory;
using Application.Use_Cases.Category.DeleteCategory;
using Application.Use_Cases.Category.GetAllCategories;
using Application.Use_Cases.Category.GetCategory;
using Application.Use_Cases.Category.UpdateCategory;
using Application.Use_Cases.User.DeleteUser;
using Application.Use_Cases.User.GetAllUsers;
using Application.Use_Cases.User.GetUserProfile;
using Application.Use_Cases.User.ChangePassword;
using Application.Use_Cases.User.DTOs;
using Application.Use_Cases.User.Validators;
using Application.Use_Cases.User.UpdateUserProfile;
using Application.Use_Cases.Course.CreateCourse;
using Application.Use_Cases.Course.GetAllCourses;
using Application.Use_Cases.Course.GetAllPublishedCourses;
using Application.Use_Cases.Course.Manage_Courses.GetMyCourses;
using Application.Use_Cases.Course.Manage_Courses.PublishCourse;
using Application.Use_Cases.Course.Manage_Courses.UnpublishCourse;
using Application.Use_Cases.Course.Manage_Courses.DeleteCourse;
using Application.Use_Cases.Course.Manage_Courses.UpdateCourse;
using Application.Use_Cases.Course.EnrollInCourse;
using Application.Use_Cases.Course.GetMyEnrolledCourses;
using Application.Use_Cases.Chapter.CreateChapter;
using Application.Use_Cases.Chapter.DeleteChapter;
using Application.Use_Cases.Chapter.UpdateChapter;
using Application.Use_Cases.Chapter.ReorderChapters;
using Application.Use_Cases.Rating.AddRating;
using Application.Use_Cases.Rating.DeleteRating;
using Application.Use_Cases.Rating.UpdateRating;
using Application.Use_Cases.Rating.GetCourseRatings;
using Application.Use_Cases.Rating.GetUserRatingForCourse;
using Application.Use_Cases.Rating.GetAverageCourseRating;
using Application.Use_Cases.Lesson.CreateLesson;
using Application.Use_Cases.Lesson.DeleteLesson;
using Application.Use_Cases.Lesson.DTOs;
using Application.Use_Cases.Lesson.GetLesson;
using Application.Use_Cases.Lesson.GetLessonsForChapter;
using Application.Use_Cases.Lesson.ReorderLessons;
using Application.Use_Cases.Lesson.UpdateLesson;
using Application.Use_Cases.Lesson.Validators;
using Application.Use_Cases.Lesson;
using Application.Use_Cases.Course.GetCourseWithDetails;
using Application.Use_Cases.Question.CreateQuestion;
using Application.Use_Cases.Question.GetQuestion;
using Application.Use_Cases.Question.UpdateQuestion;
using Application.Use_Cases.Question.DeleteQuestion;
using Application.Use_Cases.Question.ReorderQuestions;
using Application.Use_Cases.Question.GetQuestionsForLesson;
using Application.Use_Cases.AnswerOption.CreateAnswerOption;
using Application.Use_Cases.AnswerOption.DeleteAnswerOption;
using Application.Use_Cases.AnswerOption.DTOs;
using Application.Use_Cases.AnswerOption.GetAnswerOption;
using Application.Use_Cases.AnswerOption.GetAnswerOptionsForQuestion;
using Application.Use_Cases.AnswerOption.UpdateAnswerOption;
using Application.Use_Cases.AnswerOption.Validators;
using Application.Use_Cases.UserProgress.SubmitAnswer;
using Application.Use_Cases.UserProgress.CompleteLesson;
using Application.Use_Cases.UserProgress.GetCourseProgress;
using Application.Use_Cases.UserProgress;
using Application.Use_Cases.UserProgress.ResetLessonProgress;

namespace WebAPI
{
    public static class ServiceExtensions
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging(true);  
            });
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUserCourseRepository, UserCourseRepository>();
            services.AddScoped<IChapterRepository, ChapterRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<IAnswerOptionRepository, AnswerOptionRepository>();
            services.AddScoped<IUserProgressRepository, UserProgressRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICreateRoleUseCase, CreateRoleUseCase>();
            services.AddScoped<IRegisterUseCase, RegisterUseCase>();
            services.AddScoped<ILogInUseCase, LogInUseCase>();
            services.AddValidatorsFromAssemblyContaining<RoleValidator>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<IAuthSettings, AuthSettings>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
            services.AddScoped<ILogOutUseCase, LogOutUseCase>();
            services.AddScoped<IForgotPasswordUseCase, ForgotPasswordUseCase>();
            services.AddScoped<IEmailService, MailpitEmailService>();
            services.AddScoped<IPasswordResetTokenGenerator, PasswordResetTokenGenerator>();
            services.AddScoped<ISetNewPasswordUseCase, SetNewPasswordUseCase>();
            services.AddScoped<IGetRolesUseCase, GetRolesUseCase>();
            services.AddScoped<IGetUserRolesUseCase, GetUserRolesUseCase>();
            services.AddScoped<IAssignRoleUseCase,AssignRoleUseCase>();
            services.AddScoped<IRemoveRoleUseCase, RemoveRoleUseCase>();
            services.AddScoped<IUpdateRoleUseCase, UpdateRoleUseCase>();
            services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
            services.AddScoped<IDeleteCategoryUseCase, DeleteCategoryUseCase>();
            services.AddScoped<IGetCategoriesUseCase, GetCategoriesUseCase>();
            services.AddScoped<IGetCategoryUseCase, GetCategoryUseCase>();
            services.AddScoped<IUpdateCategoryUseCase, UpdateCategoryUseCase>();
            services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
            services.AddHttpContextAccessor();
            services.AddScoped<IGetAllUsersUseCase, GetAllUsersUseCase>();
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddScoped<IUpdateUserProfileUseCase, UpdateUserProfileUseCase>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICreateCourseUseCase, CreateCourseUseCase>();
            services.AddScoped<IGetPublishedCoursesUseCase, GetPublishedCoursesUseCase>();
            services.AddScoped<IGetMyCoursesUseCase, GetMyCoursesUseCase>();
            services.AddScoped<IPublishCourseUseCase, PublishCourseUseCase>();
            services.AddScoped<IUnpublishCourseUseCase, UnpublishCourseUseCase>();
            services.AddScoped<IDeleteCourseUseCase, DeleteCourseUseCase>();
            services.AddScoped<IUpdateCourseUseCase, UpdateCourseUseCase>();
            services.AddScoped<IEnrollInCourseUseCase, EnrollInCourseUseCase>();
            services.AddScoped<IGetMyEnrolledCoursesUseCase, GetMyEnrolledCoursesUseCase>();
            services.AddScoped<ICreateChapterUseCase, CreateChapterUseCase>();
            services.AddScoped<IDeleteChapterUseCase, DeleteChapterUseCase>();
            services.AddScoped<IUpdateChapterUseCase, UpdateChapterUseCase>();
            services.AddScoped<IReorderChaptersUseCase, ReorderChaptersUseCase>();
            services.AddScoped<IAddRatingUseCase, AddRatingUseCase>();
            services.AddScoped<IDeleteRatingUseCase, DeleteRatingUseCase>();
            services.AddScoped<IUpdateRatingUseCase, UpdateRatingUseCase>();
            services.AddScoped<IGetCourseRatingsUseCase, GetCourseRatingsUseCase>();
            services.AddScoped<IGetUserRatingForCourseUseCase, GetUserRatingForCourseUseCase>();
            services.AddScoped<IGetAverageCourseRatingUseCase, GetAverageCourseRatingUseCase>();
            services.AddScoped<ICreateLessonUseCase, CreateLessonUseCase>();
            services.AddScoped<IGetLessonUseCase, GetLessonUseCase>();
            services.AddScoped<IUpdateLessonUseCase, UpdateLessonUseCase>();
            services.AddScoped<IDeleteLessonUseCase, DeleteLessonUseCase>();
            services.AddScoped<IReorderLessonsUseCase, ReorderLessonsUseCase>();
            services.AddScoped<IGetLessonsForChapterUseCase, GetLessonsForChapterUseCase>();
            services.AddScoped<IGetCourseWithDetailsUseCase, GetCourseWithDetailsUseCase>();
            services.AddScoped<IGetQuestionUseCase, GetQuestionUseCase>();
            services.AddScoped<IDeleteQuestionUseCase, DeleteQuestionUseCase>();
            services.AddScoped<IReorderQuestionsUseCase, ReorderQuestionsUseCase>();
            services.AddScoped<IGetQuestionsForLessonUseCase, GetQuestionsForLessonUseCase>();
            services.AddScoped<ICreateAnswerOptionUseCase, CreateAnswerOptionUseCase>();
            services.AddScoped<IGetAnswerOptionUseCase, GetAnswerOptionUseCase>();
            services.AddScoped<IUpdateAnswerOptionUseCase, UpdateAnswerOptionUseCase>();
            services.AddScoped<IDeleteAnswerOptionUseCase, DeleteAnswerOptionUseCase>();
            services.AddScoped<IGetAnswerOptionsForQuestionUseCase, GetAnswerOptionsForQuestionUseCase>();
            services.AddScoped<IUpdateMultipleChoiceQuestionUseCase, UpdateMultipleChoiceQuestionUseCase>();
            services.AddScoped<IUpdateOpenEndedQuestionUseCase, UpdateOpenEndedQuestionUseCase>();
            services.AddScoped<ICreateOpenEndedQuestionUseCase, CreateOpenEndedQuestionUseCase>();
            services.AddScoped<ICreateMultipleChoiceQuestionUseCase, CreateMultipleChoiceQuestionUseCase>();
            services.AddScoped<ISubmitMultipleChoiceAnswerUseCase, SubmitMultipleChoiceAnswerUseCase>();
            services.AddScoped<ISubmitOpenEndedAnswerUseCase, SubmitOpenEndedAnswerUseCase>();
            services.AddScoped<ICompleteLessonUseCase, CompleteLessonUseCase>();
            services.AddScoped<IGetCourseProgressUseCase, GetCourseProgressUseCase>();
            services.AddScoped<IResetLessonProgressUseCase, ResetLessonProgressUseCase>();
        }

        public static void AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(RoleProfile).Assembly);
        }

        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"]
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Authenticated", policy =>
                    policy.RequireAuthenticatedUser());

                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("admin"));

                options.AddPolicy("UserOnly", policy =>
                    policy.RequireRole("user"));

                options.AddPolicy("AuthorOnly", policy =>
                    policy.RequireRole("author"));
            });

        }

    }
}
