using ChameleonFutureAcademyAdminApi.Hateoas;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Tags;

public record HateoasResponseTagDto(
    int TagId,
    string Description
) : Resource
{
    public static HateoasResponseTagDto From(Tag tag) =>
        new(
            tag.TagId,
            tag.Description
        );
}