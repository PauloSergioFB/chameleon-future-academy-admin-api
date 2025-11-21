using ChameleonFutureAcademyAdminApi.Hateoas;

namespace ChameleonFutureAcademyAdminApi.Services;

public class HateoasService(LinkGenerator link)
{

    private readonly LinkGenerator _link = link;

    public Link Build(string routeName, object? values, string rel, string method, HttpContext ctx)
    {
        var href = _link.GetUriByName(ctx, routeName, values);
        return new Link(href ?? "", rel, method);
    }

}