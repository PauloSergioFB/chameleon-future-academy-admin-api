using ChameleonFutureAcademyAdminApi.Hateoas;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Courses;

public record HateoasResponseCourseDto(
    int CourseId,
    string? Title,
    string? Description,
    string? Author,
    string? ThumbnailUrl,
    DateTime CreatedAt
) : Resource
{
    public static HateoasResponseCourseDto From(Course course) =>
        new(
            course.CourseId,
            course.Title,
            course.Description,
            course.Author,
            course.ThumbnailUrl,
            course.CreatedAt
        );
}