using ChameleonFutureAcademyAdminApi.Models;
using ChameleonFutureAcademyAdminApi.Hateoas;

namespace ChameleonFutureAcademyAdminApi.DTOs.Activities;

public record ResponseActivityDto(
    int ActivityId,
    int ContentId,
    string Title,
    string Body,
    string Explanation,
    DateTime CreatedAt
) : Resource
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