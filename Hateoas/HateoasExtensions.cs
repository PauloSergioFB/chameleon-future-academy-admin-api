using ChameleonFutureAcademyAdminApi.Services;

namespace ChameleonFutureAcademyAdminApi.Hateoas;

public static class HateoasExtensions
{
    public static T AddLinks<T>(
        this T resource,
        HttpContext ctx,
        Func<HateoasService, HttpContext, IEnumerable<Link>> linkBuilder)
        where T : Resource
    {
        var svc = ctx.RequestServices.GetRequiredService<HateoasService>();
        resource.Links = [.. linkBuilder(svc, ctx)];
        return resource;
    }
}