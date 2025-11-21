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

        var rawQuery = ctx.Request.QueryString.Value ?? "";
        var qs = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(rawQuery);

        var baseParams = qs.ToDictionary(
            x => x.Key,
            x => (object)x.Value.ToString()
        );

        baseParams["page"] = page;
        baseParams["size"] = size;

        links.Add(gen.Build(routeName, baseParams, "self", "GET", ctx));

        if (page > 1)
            links.Add(gen.Build(
                routeName,
                new Dictionary<string, object>(baseParams) { ["page"] = page - 1 },
                "prev", "GET", ctx)
            );

        if (page < totalPages)
            links.Add(gen.Build(
                routeName,
                new Dictionary<string, object>(baseParams) { ["page"] = page + 1 },
                "next", "GET", ctx)
            );

        return links;
    }

}