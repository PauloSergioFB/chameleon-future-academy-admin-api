using ChameleonFutureAcademyAdminApi.DTOs.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ChameleonFutureAcademyAdminApi.Services;

public class PaginationService
{

    public async Task<PagedResult<T>> CreatePagedResultAsync<T>(
        IQueryable<T> query,
        int page,
        int size)
    {
        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        return new(items, page, size, totalItems, totalPages);
    }

}