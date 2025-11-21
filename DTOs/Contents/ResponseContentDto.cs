using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Contents;

public record ResponseContentDto(
    int ContentId,
    int CourseId,
    string Type,
    int Position
)
{
    public static ResponseContentDto From(Content content) =>
        new(
            content.ContentId,
            content.CourseId,
            content.Type,
            content.Position
        );
}