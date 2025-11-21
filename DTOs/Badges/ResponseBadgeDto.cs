using ChameleonFutureAcademyAdminApi.Hateoas;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Badges;

public record ResponseBadgeDto(
    int BadgeId,
    int CourseId,
    string Title,
    string IconUrl
) : Resource
{
    public static ResponseBadgeDto From(Badge badge) =>
        new(
            badge.BadgeId,
            badge.CourseId,
            badge.Title,
            badge.IconUrl
        );
}