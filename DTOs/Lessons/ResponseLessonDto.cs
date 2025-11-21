using ChameleonFutureAcademyAdminApi.Hateoas;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Lessons;

public record ResponseLessonDto(
    int LessonId,
    int ContentId,
    string Title,
    string Body,
    DateTime CreatedAt
) : Resource
{
    public static ResponseLessonDto From(Lesson lesson) =>
        new(
            lesson.LessonId,
            lesson.ContentId,
            lesson.Title,
            lesson.Body,
            lesson.CreatedAt
        );
}