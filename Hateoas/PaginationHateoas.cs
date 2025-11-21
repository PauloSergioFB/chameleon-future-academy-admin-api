using ChameleonFutureAcademyAdminApi.Services;

namespace ChameleonFutureAcademyAdminApi.Hateoas;

public static class PaginationHateoas
{

    public static List<Link> BuildPaginationLinks(
        string routeName,
        int page,
        int size,
        int totalPages,
        HttpContext ctx)
    {
        var links = new List<Link>();

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        links.Add(gen.Build(routeName, new { page, size }, "self", "GET", ctx));

        if (page > 1)
        {
            links.Add(gen.Build(routeName, new { page = page - 1, size }, "prev", "GET", ctx));
        }

        if (page < totalPages)
        {
            links.Add(gen.Build(routeName, new { page = page + 1, size }, "next", "GET", ctx));
        }

        return links;
    }

}
