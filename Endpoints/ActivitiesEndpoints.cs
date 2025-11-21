using Microsoft.AspNetCore.Http.HttpResults;
using ChameleonFutureAcademyAdminApi.Data;
using ChameleonFutureAcademyAdminApi.DTOs.Activities;
using ChameleonFutureAcademyAdminApi.DTOs.Pagination;
using ChameleonFutureAcademyAdminApi.Models;
using ChameleonFutureAcademyAdminApi.Services;
using ChameleonFutureAcademyAdminApi.Hateoas;
using Microsoft.AspNetCore.Mvc;
using ChameleonFutureAcademyAdminApi.Filters;

namespace ChameleonFutureAcademyAdminApi.Endpoints;

public static class ActivitiesEndpoints
{

    public static void MapActivitiesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/activities").WithTags("Atividades");

        group.MapGet("/", GetAllActivities)
            .WithName("GetAllActivities")
            .WithSummary("Lista atividades")
            .WithDescription("Retorna todas as atividades cadastradas, com suporte a paginação.")
            .Produces<Ok<PageResponse<ResponseActivityDto>>>(StatusCodes.Status200OK);

        group.MapGet("/search", SearchActivities)
            .WithName("SearchActivities")
            .WithSummary("Busca atividades")
            .WithDescription("Busca atividades filtrando pelo ID do conteúdo ao qual pertencem.")
            .Produces<Ok<PageResponse<ResponseActivityDto>>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", GetActivityById)
            .WithName("GetActivityById")
            .WithSummary("Obtém uma atividade específica")
            .WithDescription("Retorna as informações de uma atividade identificada pelo ID.")
            .Produces<Ok<HateoasResponseActivityDto>>(StatusCodes.Status200OK)
            .Produces<NotFound>(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateActivity)
            .WithName("CreateActivity")
            .WithSummary("Cria uma atividade")
            .WithDescription("Cria uma nova atividade vinculada a um conteúdo.")
            .AddEndpointFilter<ValidationFilter<CreateActivityDto>>()
            .Produces<Created<HateoasResponseActivityDto>>(StatusCodes.Status201Created)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", UpdateActivity)
            .WithName("UpdateActivity")
            .WithSummary("Atualiza uma atividade")
            .WithDescription("Atualiza os dados de uma atividade existente identificada pelo ID.")
            .AddEndpointFilter<ValidationFilter<CreateActivityDto>>()
            .Produces<Ok<HateoasResponseActivityDto>>(StatusCodes.Status200OK)
            .Produces<NotFound>(StatusCodes.Status404NotFound)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", DeleteActivity)
            .WithName("DeleteActivity")
            .WithSummary("Exclui uma atividade")
            .WithDescription("Remove permanentemente a atividade identificada pelo ID.")
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<NotFound>(StatusCodes.Status404NotFound);
    }

    static async Task<Ok<PageResponse<ResponseActivityDto>>> GetAllActivities(
        [FromServices] AppDbContext db,
        [FromServices] PaginationService paginator,
        HttpContext ctx,
        int page = 1,
        int size = 20)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = db.Activities
            .OrderBy(r => r.ActivityId).Select(r => ResponseActivityDto.From(r));

        var result = await paginator.CreatePagedResultAsync(query, page, size);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        var links = PaginationHateoas.BuildPaginationLinks("GetAllActivities", page, size, result.TotalPages, ctx);
        links.Add(gen.Build("SearchActivities", null, "search", "GET", ctx));
        links.Add(gen.Build("CreateActivity", null, "create", "POST", ctx));

        result.Links = links;
        return TypedResults.Ok(result);
    }

    static async Task<Ok<PageResponse<ResponseActivityDto>>> SearchActivities(
        [FromServices] AppDbContext db,
        [FromServices] PaginationService paginator,
        HttpContext ctx,
        int? content_id,
        int page = 1,
        int size = 20)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = db.Activities
            .Where(r => r.ContentId == content_id)
            .OrderBy(r => r.ActivityId)
            .Select(r => ResponseActivityDto.From(r));

        var result = await paginator.CreatePagedResultAsync(query, page, size);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        var links = PaginationHateoas.BuildPaginationLinks("SearchActivities", page, size, result.TotalPages, ctx);
        links.Add(gen.Build("GetAllActivities", null, "all", "GET", ctx));
        links.Add(gen.Build("CreateActivity", null, "create", "POST", ctx));

        result.Links = links;
        return TypedResults.Ok(result);
    }

    static async Task<Results<Ok<HateoasResponseActivityDto>, NotFound>> GetActivityById(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id)
    {
        var result = await db.Activities.FindAsync(id);
        if (result is null) return TypedResults.NotFound();

        var dto = HateoasResponseActivityDto.From(result);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        dto.Links = [
            gen.Build("GetActivityById", new { id = result.ActivityId }, "self", "GET", ctx),
            gen.Build("GetAllActivities", null, "all", "GET", ctx),
            gen.Build("SearchActivities", null, "search", "GET", ctx),
            gen.Build("CreateActivity", null, "create", "POST", ctx),
            gen.Build("UpdateActivity", new { id = result.ActivityId }, "update", "PUT", ctx),
            gen.Build("DeleteActivity", new { id = result.ActivityId }, "delete", "DELETE", ctx)
        ];

        return TypedResults.Ok(dto);
    }

    static async Task<Results<Created<HateoasResponseActivityDto>, BadRequest>> CreateActivity(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        CreateActivityDto requestBody)
    {
        Activity activity = requestBody.ToEntity();

        try
        {
            await db.Activities.AddAsync(activity);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return TypedResults.BadRequest();
        }

        var response = HateoasResponseActivityDto.From(activity);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        response.Links = [
            gen.Build("GetActivityById", new { id = activity.ActivityId }, "self", "GET", ctx),
            gen.Build("UpdateActivity", new { id = activity.ActivityId }, "update", "PUT", ctx),
            gen.Build("DeleteActivity", new { id = activity.ActivityId }, "delete", "DELETE", ctx),
            gen.Build("GetAllActivities", null, "all", "GET", ctx),
            gen.Build("SearchActivities", null, "search", "GET", ctx),
            gen.Build("CreateActivity", null, "create", "POST", ctx)
        ];

        return TypedResults.Created(
            $"/api/activities/{activity.ActivityId}",
            response
        );
    }

    static async Task<Results<Ok<HateoasResponseActivityDto>, NotFound, BadRequest>> UpdateActivity(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id,
        CreateActivityDto requestBody)
    {
        var entity = await db.Activities.FindAsync(id);
        if (entity is null) return TypedResults.NotFound();

        try
        {
            requestBody.ApplyToEntity(entity);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return TypedResults.BadRequest();
        }

        var response = HateoasResponseActivityDto.From(entity);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        response.Links = [
            gen.Build("GetActivityById", new { id = entity.ActivityId }, "self", "GET", ctx),
            gen.Build("UpdateActivity", new { id = entity.ActivityId }, "update", "PUT", ctx),
            gen.Build("DeleteActivity", new { id = entity.ActivityId }, "delete", "DELETE", ctx),
            gen.Build("GetAllActivities", null, "all", "GET", ctx),
            gen.Build("SearchActivities", null, "search", "GET", ctx),
            gen.Build("CreateActivity", null, "create", "POST", ctx)
        ];

        return TypedResults.Ok(response);
    }

    static async Task<Results<NoContent, NotFound>> DeleteActivity(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id)
    {
        var result = await db.Activities.FindAsync(id);
        if (result is null) return TypedResults.NotFound();

        db.Activities.Remove(result);
        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

}