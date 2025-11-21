using Microsoft.AspNetCore.Http.HttpResults;
using ChameleonFutureAcademyAdminApi.Data;
using ChameleonFutureAcademyAdminApi.DTOs.Contents;
using ChameleonFutureAcademyAdminApi.DTOs.Pagination;
using ChameleonFutureAcademyAdminApi.Models;
using ChameleonFutureAcademyAdminApi.Services;
using ChameleonFutureAcademyAdminApi.Hateoas;
using Microsoft.AspNetCore.Mvc;
using ChameleonFutureAcademyAdminApi.Filters;

namespace ChameleonFutureAcademyAdminApi.Endpoints;

public static class ContentsEndpoints
{

    public static void MapContentsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/contents").WithTags("Conteúdos");

        group.MapGet("/", GetAllContents)
            .WithName("GetAllContents")
            .WithSummary("Lista conteúdos")
            .WithDescription("Retorna todos os conteúdos cadastrados, com suporte a paginação.")
            .Produces<Ok<PageResponse<ResponseContentDto>>>(StatusCodes.Status200OK);

        group.MapGet("/search", SearchContents)
            .WithName("SearchContents")
            .WithSummary("Busca conteúdos")
            .WithDescription("Busca conteúdos filtrando pelo ID do curso ao qual pertencem.")
            .Produces<Ok<PageResponse<ResponseContentDto>>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", GetContentById)
            .WithName("GetContentById")
            .WithSummary("Obtém um conteúdo específico")
            .WithDescription("Retorna as informações de um conteúdo identificado pelo ID.")
            .Produces<Ok<HateoasResponseContentDto>>(StatusCodes.Status200OK)
            .Produces<NotFound>(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateContent)
            .WithName("CreateContent")
            .WithSummary("Cria um conteúdo")
            .WithDescription("Cria um novo conteúdo associado a um curso, definindo tipo e posição.")
            .AddEndpointFilter<ValidationFilter<CreateContentDto>>()
            .Produces<Created<HateoasResponseContentDto>>(StatusCodes.Status201Created)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", UpdateContent)
            .WithName("UpdateContent")
            .WithSummary("Atualiza um conteúdo")
            .WithDescription("Atualiza os dados de um conteúdo existente identificado pelo ID.")
            .AddEndpointFilter<ValidationFilter<CreateContentDto>>()
            .Produces<Ok<HateoasResponseContentDto>>(StatusCodes.Status200OK)
            .Produces<NotFound>(StatusCodes.Status404NotFound)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", DeleteContent)
            .WithName("DeleteContent")
            .WithSummary("Exclui um conteúdo")
            .WithDescription("Exclui permanentemente o conteúdo identificado pelo ID.")
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<NotFound>(StatusCodes.Status404NotFound);
    }

    static async Task<Ok<PageResponse<ResponseContentDto>>> GetAllContents(
        [FromServices] AppDbContext db,
        [FromServices] PaginationService paginator,
        HttpContext ctx,
        int page = 1,
        int size = 20)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = db.Contents
            .OrderBy(r => r.ContentId).Select(r => ResponseContentDto.From(r));

        var result = await paginator.CreatePagedResultAsync(query, page, size);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        var links = PaginationHateoas.BuildPaginationLinks("GetAllContents", page, size, result.TotalPages, ctx);
        links.Add(gen.Build("SearchContents", null, "search", "GET", ctx));
        links.Add(gen.Build("CreateContent", null, "create", "POST", ctx));

        result.Links = links;
        return TypedResults.Ok(result);
    }

    static async Task<Ok<PageResponse<ResponseContentDto>>> SearchContents(
        [FromServices] AppDbContext db,
        [FromServices] PaginationService paginator,
        HttpContext ctx,
        int? content_id,
        int page = 1,
        int size = 20)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = db.Contents
            .Where(r => r.ContentId == content_id)
            .OrderBy(r => r.ContentId)
            .Select(r => ResponseContentDto.From(r));

        var result = await paginator.CreatePagedResultAsync(query, page, size);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        var links = PaginationHateoas.BuildPaginationLinks("SearchContents", page, size, result.TotalPages, ctx);
        links.Add(gen.Build("GetAllContents", null, "all", "GET", ctx));
        links.Add(gen.Build("CreateContent", null, "create", "POST", ctx));

        result.Links = links;
        return TypedResults.Ok(result);
    }

    static async Task<Results<Ok<HateoasResponseContentDto>, NotFound>> GetContentById(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id)
    {
        var result = await db.Contents.FindAsync(id);
        if (result is null) return TypedResults.NotFound();

        var dto = HateoasResponseContentDto.From(result);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        dto.Links = [
            gen.Build("GetContentById", new { id = result.ContentId }, "self", "GET", ctx),
            gen.Build("GetAllContents", null, "all", "GET", ctx),
            gen.Build("SearchContents", null, "search", "GET", ctx),
            gen.Build("CreateContent", null, "create", "POST", ctx),
            gen.Build("UpdateContent", new { id = result.ContentId }, "update", "PUT", ctx),
            gen.Build("DeleteContent", new { id = result.ContentId }, "delete", "DELETE", ctx)
        ];

        return TypedResults.Ok(dto);
    }

    static async Task<Results<Created<HateoasResponseContentDto>, BadRequest>> CreateContent(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        CreateContentDto requestBody)
    {
        Content content = requestBody.ToEntity();

        try
        {
            await db.Contents.AddAsync(content);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return TypedResults.BadRequest();
        }

        var response = HateoasResponseContentDto.From(content);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        response.Links = [
            gen.Build("GetContentById", new { id = content.ContentId }, "self", "GET", ctx),
            gen.Build("UpdateContent", new { id = content.ContentId }, "update", "PUT", ctx),
            gen.Build("DeleteContent", new { id = content.ContentId }, "delete", "DELETE", ctx),
            gen.Build("GetAllContents", null, "all", "GET", ctx),
            gen.Build("SearchContents", null, "search", "GET", ctx),
            gen.Build("CreateContent", null, "create", "POST", ctx)
        ];

        return TypedResults.Created(
            $"/api/contents/{content.ContentId}",
            response
        );
    }

    static async Task<Results<Ok<HateoasResponseContentDto>, NotFound, BadRequest>> UpdateContent(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id,
        CreateContentDto requestBody)
    {
        var entity = await db.Contents.FindAsync(id);
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

        var response = HateoasResponseContentDto.From(entity);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        response.Links = [
            gen.Build("GetContentById", new { id = entity.ContentId }, "self", "GET", ctx),
            gen.Build("UpdateContent", new { id = entity.ContentId }, "update", "PUT", ctx),
            gen.Build("DeleteContent", new { id = entity.ContentId }, "delete", "DELETE", ctx),
            gen.Build("GetAllContents", null, "all", "GET", ctx),
            gen.Build("SearchContents", null, "search", "GET", ctx),
            gen.Build("CreateContent", null, "create", "POST", ctx)
        ];

        return TypedResults.Ok(response);
    }

    static async Task<Results<NoContent, NotFound>> DeleteContent(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id)
    {
        var result = await db.Contents.FindAsync(id);
        if (result is null) return TypedResults.NotFound();

        db.Contents.Remove(result);
        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

}