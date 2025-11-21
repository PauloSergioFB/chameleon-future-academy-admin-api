using ChameleonFutureAcademyAdminApi.Hateoas;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.ActivityOptions;

public record ResponseActivityOptionDto(
    int ActivityOptionId,
    int ActivityId,
    string Label,
    string Description,
    bool IsCorrect
) : Resource
{
    public static ResponseActivityOptionDto From(ActivityOption option) =>
        new(
            option.ActivityOptionId,
            option.ActivityId,
            option.Label,
            option.Description,
            option.IsCorrect
        );
}