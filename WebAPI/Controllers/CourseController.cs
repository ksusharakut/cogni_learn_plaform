using Application.Use_Cases.Course.CreateCourse;
using Application.Use_Cases.Course.DTOs;
using Application.Use_Cases.Course.GetAllCourses;
using Application.Use_Cases.Course.GetCourseWithDetails;
using Application.Use_Cases.Course.Manage_Courses.DeleteCourse;
using Application.Use_Cases.Course.Manage_Courses.GetMyCourses;
using Application.Use_Cases.Course.Manage_Courses.PublishCourse;
using Application.Use_Cases.Course.Manage_Courses.UnpublishCourse;
using Application.Use_Cases.Course.Manage_Courses.UpdateCourse;
using Application.Use_Cases.Role.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICreateCourseUseCase _createcourseUseCase;
        private readonly IGetPublishedCoursesUseCase _getPublishedCoursesUseCase;
        private readonly IGetMyCoursesUseCase _getMyCoursesUseCase;
        private readonly IPublishCourseUseCase _publishCourseUseCase;
        private readonly IUnpublishCourseUseCase _unpublishCourseUseCase;
        private readonly IDeleteCourseUseCase _deleteCourseUseCase;
        private readonly IUpdateCourseUseCase _updateCourseUseCase;
        private readonly IGetCourseWithDetailsUseCase _getCourseWithDetailsUseCase;

        public CourseController(ICreateCourseUseCase createCourseUseCase,
            IGetPublishedCoursesUseCase getPublishedCoursesUseCase,
            IGetMyCoursesUseCase getMyCoursesUseCase,
            IPublishCourseUseCase publishCourseUseCase,
            IUnpublishCourseUseCase unpublishCourseUseCase,
            IDeleteCourseUseCase deleteCourseUseCase,
            IUpdateCourseUseCase updateCourseUseCase,
            IGetCourseWithDetailsUseCase getCourseWithDetailsUseCase)
        {
            _createcourseUseCase = createCourseUseCase;
            _getPublishedCoursesUseCase = getPublishedCoursesUseCase;
            _getMyCoursesUseCase = getMyCoursesUseCase;
            _publishCourseUseCase = publishCourseUseCase;
            _unpublishCourseUseCase= unpublishCourseUseCase;
            _deleteCourseUseCase = deleteCourseUseCase;
            _updateCourseUseCase = updateCourseUseCase;
            _getCourseWithDetailsUseCase = getCourseWithDetailsUseCase;
        }

        [HttpPost]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDTO request, CancellationToken cancellationToken)
        {
            await _createcourseUseCase.ExecuteAsync(request, cancellationToken);
            return Ok("Курс успешно создан.");
        }

        [HttpGet("published")]
        public async Task<IActionResult> GetPublishedCourses(CancellationToken cancellationToken)
        {
            var courses = await _getPublishedCoursesUseCase.ExecuteAsync(cancellationToken);
            return Ok(courses);
        }

        [HttpGet("my")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> GetMyCourses(CancellationToken cancellationToken)
        {
            var courses = await _getMyCoursesUseCase.ExecuteAsync(cancellationToken);
            return Ok(courses);
        }

        [HttpPut("{courseId}/publish")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> PublishCourse(int courseId, CancellationToken cancellationToken)
        {
            await _publishCourseUseCase.ExecuteAsync(courseId, cancellationToken);
            return Ok("Курс успешно опубликован!");
        }

        [HttpPut("{courseId}/unpublish")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> UnpublishCourse(int courseId, CancellationToken cancellationToken)
        {
            await _unpublishCourseUseCase.ExecuteAsync(courseId, cancellationToken);
            return Ok("Курс был успешно скрыт!");
        }

        [HttpDelete("{courseId}")]
        [Authorize(Policy = "AuthorOnly")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteCourse(int courseId, CancellationToken cancellationToken)
        {
            await _deleteCourseUseCase.ExecuteAsync(courseId, cancellationToken);
            return Ok("Курс успешно удалён!");
        }

        [HttpPut("{courseId}/update")]
        [Authorize(Policy = "AuthorOnly")]
        public async Task<IActionResult> UpdateCourse(int courseId, CreateCourseDTO request, CancellationToken cancellationToken)
        {
            await _updateCourseUseCase.ExecuteAsync(courseId, request, cancellationToken);
            return Ok("Курс успешно обновлён!");
        }

        [HttpGet("{courseId}/details")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> GetCourseWithDetails(int courseId, CancellationToken cancellationToken)
        {
            var courseDetails = await _getCourseWithDetailsUseCase.ExecuteAsync(courseId, cancellationToken);
            return Ok(courseDetails);
        }
    }
}
