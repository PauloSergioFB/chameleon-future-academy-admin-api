using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Activities;

public record ResponseActivityDto(
    int ActivityId,
    int ContentId,
    string Title,
    string Body,
    string Explanation,
    DateTime CreatedAt
)
{
    public static ResponseActivityDto From(Activity activity) =>
        new(
            activity.ActivityId,
            activity.ContentId,
            activity.Title,
            activity.Body,
            activity.Explanation,
            activity.CreatedAt
        );
}