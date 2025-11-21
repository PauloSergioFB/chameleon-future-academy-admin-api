using ChameleonFutureAcademyAdminApi.Hateoas;

namespace ChameleonFutureAcademyAdminApi.DTOs.Pagination;

public record PageResponse<T>(
    IEnumerable<T> Items,
    int Page,
    int Size,
    int TotalItems,
    int TotalPages
) : Resource;