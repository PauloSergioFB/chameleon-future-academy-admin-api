using ChameleonFutureAcademyAdminApi.Hateoas;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Tags;

public record ResponseTagDto(
    int TagId,
    string Description
) : Resource
{
    public static ResponseTagDto From(Tag tag) =>
        new(
            tag.TagId,
            tag.Description
        );
}