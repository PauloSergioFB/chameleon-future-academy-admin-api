using ChameleonFutureAcademyAdminApi.Hateoas;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Badges;

public record HateoasResponseBadgeDto(
    int BadgeId,
    int CourseId,
    string Title,
    string IconUrl
) : Resource
{
    public static HateoasResponseBadgeDto From(Badge badge) =>
        new(
            badge.BadgeId,
            badge.CourseId,
            badge.Title,
            badge.IconUrl
        );
}