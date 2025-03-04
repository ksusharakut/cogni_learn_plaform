using Application.Use_Cases.Course.EnrollInCourse;
using Application.Use_Cases.Course.GetMyEnrolledCourses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/user-courses")]
    [ApiController]
    public class UserCoursesController : ControllerBase
    {
        private readonly IEnrollInCourseUseCase _enrollInCourseUseCase;
        private readonly IGetMyEnrolledCoursesUseCase _getMyEnrolledCoursesUseCase;

        public UserCoursesController(IEnrollInCourseUseCase enrollInCourseUseCase,
            IGetMyEnrolledCoursesUseCase getMyEnrolledCoursesUseCase)
        {
            _enrollInCourseUseCase = enrollInCourseUseCase;
            _getMyEnrolledCoursesUseCase = getMyEnrolledCoursesUseCase;
        }

        [HttpPost("enroll/{courseId}")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> EnrollInCourse(int courseId, CancellationToken cancellationToken)
        {
            await _enrollInCourseUseCase.ExecuteAsync(courseId, cancellationToken);
            return Ok("Вы успешно записались на курс!");
        }

        [HttpGet("my/enrolled")]
        [Authorize(Policy = "Authenticated")]
        public async Task<IActionResult> GetMyEnrolledCourses(CancellationToken cancellationToken)
        {
            var courses = await _getMyEnrolledCoursesUseCase.ExecuteAsync(cancellationToken);
            return Ok(courses);
        }
    }
}
