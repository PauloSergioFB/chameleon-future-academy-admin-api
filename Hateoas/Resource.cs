namespace ChameleonFutureAcademyAdminApi.Hateoas;

public abstract record Resource
{
    public List<Link> Links { get; set; } = [];
}