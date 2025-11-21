using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ChameleonFutureAcademyAdminApi.Data;
using ChameleonFutureAcademyAdminApi.DTOs.Badges;
using ChameleonFutureAcademyAdminApi.DTOs.Pagination;
using ChameleonFutureAcademyAdminApi.Models;
using ChameleonFutureAcademyAdminApi.Services;
using ChameleonFutureAcademyAdminApi.Hateoas;
using Microsoft.AspNetCore.Mvc;
using ChameleonFutureAcademyAdminApi.Filters;

namespace ChameleonFutureAcademyAdminApi.Endpoints;

public static class BadgesEndpoints
{

    public static void MapBadgesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/badges").WithTags("Badges");

        group.MapGet("/", GetAllBadges)
            .WithName("GetAllBadges")
            .WithSummary("Lista badges")
            .WithDescription("Retorna todas as badges cadastradas, com suporte a paginação.")
            .Produces<Ok<PageResponse<ResponseBadgeDto>>>(StatusCodes.Status200OK);

        group.MapGet("/search", SearchBadges)
            .WithName("SearchBadges")
            .WithSummary("Busca badges")
            .WithDescription("Busca badges filtrando pelo ID do curso ao qual pertencem.")
            .Produces<Ok<PageResponse<ResponseBadgeDto>>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", GetBadgeById)
            .WithName("GetBadgeById")
            .WithSummary("Obtém uma badge específica")
            .WithDescription("Retorna as informações de uma badge identificada pelo ID.")
            .Produces<Ok<HateoasResponseBadgeDto>>(StatusCodes.Status200OK)
            .Produces<NotFound>(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateBadge)
            .WithName("CreateBadge")
            .WithSummary("Cria uma badge")
            .WithDescription("Cria uma nova badge associada a um curso.")
            .AddEndpointFilter<ValidationFilter<CreateBadgeDto>>()
            .Produces<Created<HateoasResponseBadgeDto>>(StatusCodes.Status201Created)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", UpdateBadge)
            .WithName("UpdateBadge")
            .WithSummary("Atualiza uma badge")
            .WithDescription("Atualiza os dados de uma badge existente identificada pelo ID.")
            .AddEndpointFilter<ValidationFilter<CreateBadgeDto>>()
            .Produces<Ok<HateoasResponseBadgeDto>>(StatusCodes.Status200OK)
            .Produces<NotFound>(StatusCodes.Status404NotFound)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", DeleteBadge)
            .WithName("DeleteBadge")
            .WithSummary("Exclui uma badge")
            .WithDescription("Remove permanentemente a badge identificada pelo ID.")
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<NotFound>(StatusCodes.Status404NotFound);
    }

    static async Task<Ok<PageResponse<ResponseBadgeDto>>> GetAllBadges(
        [FromServices] AppDbContext db,
        [FromServices] PaginationService paginator,
        HttpContext ctx,
        int page = 1,
        int size = 20)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = db.Badges
            .OrderBy(r => r.BadgeId).Select(r => ResponseBadgeDto.From(r));

        var result = await paginator.CreatePagedResultAsync(query, page, size);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        var links = PaginationHateoas.BuildPaginationLinks("GetAllBadges", page, size, result.TotalPages, ctx);
        links.Add(gen.Build("SearchBadges", null, "search", "GET", ctx));
        links.Add(gen.Build("CreateBadge", null, "create", "POST", ctx));

        result.Links = links;
        return TypedResults.Ok(result);
    }

    static async Task<Ok<PageResponse<ResponseBadgeDto>>> SearchBadges(
        [FromServices] AppDbContext db,
        [FromServices] PaginationService paginator,
        HttpContext ctx,
        int? course_id,
        int page = 1,
        int size = 20)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = db.Badges
            .Where(r => r.CourseId == course_id)
            .OrderBy(r => r.BadgeId)
            .Select(r => ResponseBadgeDto.From(r));

        var result = await paginator.CreatePagedResultAsync(query, page, size);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        var links = PaginationHateoas.BuildPaginationLinks("SearchBadges", page, size, result.TotalPages, ctx);
        links.Add(gen.Build("GetAllBadges", null, "all", "GET", ctx));
        links.Add(gen.Build("CreateBadge", null, "create", "POST", ctx));

        result.Links = links;
        return TypedResults.Ok(result);
    }

    static async Task<Results<Ok<HateoasResponseBadgeDto>, NotFound>> GetBadgeById(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id)
    {
        var result = await db.Badges.FindAsync(id);
        if (result is null) return TypedResults.NotFound();

        var dto = HateoasResponseBadgeDto.From(result);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        dto.Links = [
            gen.Build("GetBadgeById", new { id = result.BadgeId }, "self", "GET", ctx),
            gen.Build("GetAllBadges", null, "all", "GET", ctx),
            gen.Build("SearchBadges", null, "search", "GET", ctx),
            gen.Build("CreateBadge", null, "create", "POST", ctx),
            gen.Build("UpdateBadge", new { id = result.BadgeId }, "update", "PUT", ctx),
            gen.Build("DeleteBadge", new { id = result.BadgeId }, "delete", "DELETE", ctx)
        ];

        return TypedResults.Ok(dto);
    }

    static async Task<Results<Created<HateoasResponseBadgeDto>, BadRequest>> CreateBadge(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        CreateBadgeDto requestBody)
    {
        Badge badge = requestBody.ToEntity();

        try
        {
            await db.Badges.AddAsync(badge);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return TypedResults.BadRequest();
        }

        var response = HateoasResponseBadgeDto.From(badge);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        response.Links = [
            gen.Build("GetBadgeById", new { id = badge.BadgeId }, "self", "GET", ctx),
            gen.Build("UpdateBadge", new { id = badge.BadgeId }, "update", "PUT", ctx),
            gen.Build("DeleteBadge", new { id = badge.BadgeId }, "delete", "DELETE", ctx),
            gen.Build("GetAllBadges", null, "all", "GET", ctx),
            gen.Build("SearchBadges", null, "search", "GET", ctx),
            gen.Build("CreateBadge", null, "create", "POST", ctx)
        ];

        return TypedResults.Created(
            $"/api/badges/{badge.BadgeId}",
            response
        );
    }

    static async Task<Results<Ok<HateoasResponseBadgeDto>, NotFound, BadRequest>> UpdateBadge(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id,
        CreateBadgeDto requestBody)
    {
        var entity = await db.Badges.FindAsync(id);
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

        var response = HateoasResponseBadgeDto.From(entity);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        response.Links = [
            gen.Build("GetBadgeById", new { id = entity.BadgeId }, "self", "GET", ctx),
            gen.Build("UpdateBadge", new { id = entity.BadgeId }, "update", "PUT", ctx),
            gen.Build("DeleteBadge", new { id = entity.BadgeId }, "delete", "DELETE", ctx),
            gen.Build("GetAllBadges", null, "all", "GET", ctx),
            gen.Build("SearchBadges", null, "search", "GET", ctx),
            gen.Build("CreateBadge", null, "create", "POST", ctx)
        ];

        return TypedResults.Ok(response);
    }

    static async Task<Results<NoContent, NotFound>> DeleteBadge(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id)
    {
        var result = await db.Badges.FindAsync(id);
        if (result is null) return TypedResults.NotFound();

        db.Badges.Remove(result);
        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

}