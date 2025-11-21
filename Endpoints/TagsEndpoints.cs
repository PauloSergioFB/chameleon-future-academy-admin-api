using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ChameleonFutureAcademyAdminApi.Data;
using ChameleonFutureAcademyAdminApi.DTOs.Tags;
using ChameleonFutureAcademyAdminApi.DTOs.Pagination;
using ChameleonFutureAcademyAdminApi.Models;
using ChameleonFutureAcademyAdminApi.Services;
using ChameleonFutureAcademyAdminApi.Hateoas;
using Microsoft.AspNetCore.Mvc;
using ChameleonFutureAcademyAdminApi.Filters;

namespace ChameleonFutureAcademyAdminApi.Endpoints;

public static class TagsEndpoints
{

    public static void MapTagsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/tags").WithTags("Tags");

        group.MapGet("/", GetAllTags)
            .WithName("GetAllTags")
            .WithSummary("Lista todas as tags")
            .WithDescription("Retorna todas as tags cadastradas, com suporte a paginação.")
            .Produces<Ok<PageResponse<ResponseTagDto>>>(StatusCodes.Status200OK);

        group.MapGet("/search", SearchTags)
            .WithName("SearchTags")
            .WithSummary("Busca tags")
            .WithDescription("Realiza uma busca filtrando tags pela descrição fornecida.")
            .Produces<Ok<PageResponse<ResponseTagDto>>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", GetTagById)
            .WithName("GetTagById")
            .WithSummary("Obtém uma tag específica")
            .WithDescription("Retorna as informações de uma tag identificada pelo ID.")
            .Produces<Ok<HateoasResponseTagDto>>(StatusCodes.Status200OK)
            .Produces<NotFound>(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateTag)
            .WithName("CreateTag")
            .WithSummary("Cria uma nova tag")
            .WithDescription("Cria uma nova tag com os dados fornecidos.")
            .AddEndpointFilter<ValidationFilter<CreateTagDto>>()
            .Produces<Created<HateoasResponseTagDto>>(StatusCodes.Status201Created)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", UpdateTag)
            .WithName("UpdateTag")
            .WithSummary("Atualiza uma tag existente")
            .WithDescription("Atualiza os dados de uma tag identificada pelo ID.")
            .AddEndpointFilter<ValidationFilter<CreateTagDto>>()
            .Produces<Ok<HateoasResponseTagDto>>(StatusCodes.Status200OK)
            .Produces<NotFound>(StatusCodes.Status404NotFound)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", DeleteTag)
            .WithName("DeleteTag")
            .WithSummary("Exclui uma tag")
            .WithDescription("Remove permanentemente a tag identificada pelo ID.")
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<NotFound>(StatusCodes.Status404NotFound);
    }

    static async Task<Ok<PageResponse<ResponseTagDto>>> GetAllTags(
        [FromServices] AppDbContext db,
        [FromServices] PaginationService paginator,
        HttpContext ctx,
        int page = 1,
        int size = 20)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = db.Tags
            .OrderBy(r => r.TagId).Select(r => ResponseTagDto.From(r));

        var result = await paginator.CreatePagedResultAsync(query, page, size);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        var links = PaginationHateoas.BuildPaginationLinks("GetAllTags", page, size, result.TotalPages, ctx);
        links.Add(gen.Build("SearchTags", null, "search", "GET", ctx));
        links.Add(gen.Build("CreateTag", null, "create", "POST", ctx));

        result.Links = links;
        return TypedResults.Ok(result);
    }

    static async Task<Ok<PageResponse<ResponseTagDto>>> SearchTags(
        [FromServices] AppDbContext db,
        [FromServices] PaginationService paginator,
        HttpContext ctx,
        string description = "",
        int page = 1,
        int size = 20)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = db.Tags
            .Where(r => EF.Functions.Like(r.Description, $"%{description}%"))
            .OrderBy(r => r.TagId)
            .Select(r => ResponseTagDto.From(r));

        var result = await paginator.CreatePagedResultAsync(query, page, size);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        var links = PaginationHateoas.BuildPaginationLinks("SearchTags", page, size, result.TotalPages, ctx);
        links.Add(gen.Build("GetAllTags", null, "all", "GET", ctx));
        links.Add(gen.Build("CreateTag", null, "create", "POST", ctx));

        result.Links = links;
        return TypedResults.Ok(result);
    }

    static async Task<Results<Ok<HateoasResponseTagDto>, NotFound>> GetTagById(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id)
    {
        var result = await db.Tags.FindAsync(id);
        if (result is null) return TypedResults.NotFound();

        var dto = HateoasResponseTagDto.From(result);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        dto.Links = [
            gen.Build("GetTagById", new { id = result.TagId }, "self", "GET", ctx),
            gen.Build("GetAllTags", null, "all", "GET", ctx),
            gen.Build("SearchTags", null, "search", "GET", ctx),
            gen.Build("CreateTag", null, "create", "POST", ctx),
            gen.Build("UpdateTag", new { id = result.TagId }, "update", "PUT", ctx),
            gen.Build("DeleteTag", new { id = result.TagId }, "delete", "DELETE", ctx)
        ];

        return TypedResults.Ok(dto);
    }

    static async Task<Results<Created<HateoasResponseTagDto>, BadRequest>> CreateTag(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        CreateTagDto requestBody)
    {
        Tag tag = requestBody.ToEntity();

        try
        {
            await db.Tags.AddAsync(tag);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return TypedResults.BadRequest();
        }

        var response = HateoasResponseTagDto.From(tag);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        response.Links = [
            gen.Build("GetTagById", new { id = tag.TagId }, "self", "GET", ctx),
            gen.Build("UpdateTag", new { id = tag.TagId }, "update", "PUT", ctx),
            gen.Build("DeleteTag", new { id = tag.TagId }, "delete", "DELETE", ctx),
            gen.Build("GetAllTags", null, "all", "GET", ctx),
            gen.Build("SearchTags", null, "search", "GET", ctx),
            gen.Build("CreateTag", null, "create", "POST", ctx)
        ];

        return TypedResults.Created(
            $"/api/tags/{tag.TagId}",
            response
        );
    }

    static async Task<Results<Ok<HateoasResponseTagDto>, NotFound, BadRequest>> UpdateTag(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id,
        CreateTagDto requestBody)
    {
        var entity = await db.Tags.FindAsync(id);
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

        var response = HateoasResponseTagDto.From(entity);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        response.Links = [
            gen.Build("GetTagById", new { id = entity.TagId }, "self", "GET", ctx),
            gen.Build("UpdateTag", new { id = entity.TagId }, "update", "PUT", ctx),
            gen.Build("DeleteTag", new { id = entity.TagId }, "delete", "DELETE", ctx),
            gen.Build("GetAllTags", null, "all", "GET", ctx),
            gen.Build("SearchTags", null, "search", "GET", ctx),
            gen.Build("CreateTag", null, "create", "POST", ctx)
        ];

        return TypedResults.Ok(response);
    }

    static async Task<Results<NoContent, NotFound>> DeleteTag(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id)
    {
        var result = await db.Tags.FindAsync(id);
        if (result is null) return TypedResults.NotFound();

        db.Tags.Remove(result);
        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

}