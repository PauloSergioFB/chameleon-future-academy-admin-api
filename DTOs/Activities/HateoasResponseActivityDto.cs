using ChameleonFutureAcademyAdminApi.Models;
using ChameleonFutureAcademyAdminApi.Hateoas;

namespace ChameleonFutureAcademyAdminApi.DTOs.Activities;

public record HateoasResponseActivityDto(
    int ActivityId,
    int ContentId,
    string Title,
    string Body,
    string Explanation,
    DateTime CreatedAt
) : Resource
{
    public static HateoasResponseActivityDto From(Activity activity) =>
        new(
            activity.ActivityId,
            activity.ContentId,
            activity.Title,
            activity.Body,
            activity.Explanation,
            activity.CreatedAt
        );
}