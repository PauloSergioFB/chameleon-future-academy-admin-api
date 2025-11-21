using Microsoft.AspNetCore.Http.HttpResults;
using ChameleonFutureAcademyAdminApi.Data;
using ChameleonFutureAcademyAdminApi.DTOs.ActivityOptions;
using ChameleonFutureAcademyAdminApi.DTOs.Pagination;
using ChameleonFutureAcademyAdminApi.Models;
using ChameleonFutureAcademyAdminApi.Services;
using ChameleonFutureAcademyAdminApi.Hateoas;
using Microsoft.AspNetCore.Mvc;
using ChameleonFutureAcademyAdminApi.Filters;

namespace ChameleonFutureAcademyAdminApi.Endpoints;

public static class ActivityOptionsEndpoints
{

    public static void MapActivityOptionsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/activityOptions").WithTags("Alternativas");

        group.MapGet("/", GetAllActivityOptions)
            .WithName("GetAllActivityOptions")
            .WithSummary("Lista alternativas")
            .WithDescription("Retorna todas as alternativas cadastradas, com suporte a paginação.")
            .Produces<Ok<PageResponse<ResponseActivityOptionDto>>>(StatusCodes.Status200OK);

        group.MapGet("/search", SearchActivityOptions)
            .WithName("SearchActivityOptions")
            .WithSummary("Busca alternativas")
            .WithDescription("Busca alternativas filtrando pelo ID da atividade à qual pertencem.")
            .Produces<Ok<PageResponse<ResponseActivityOptionDto>>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", GetActivityOptionById)
            .WithName("GetActivityOptionById")
            .WithSummary("Obtém uma alternativa específica")
            .WithDescription("Retorna as informações de uma alternativa identificada pelo ID.")
            .Produces<Ok<HateoasResponseActivityOptionDto>>(StatusCodes.Status200OK)
            .Produces<NotFound>(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateActivityOption)
            .WithName("CreateActivityOption")
            .WithSummary("Cria uma alternativa")
            .WithDescription("Cria uma nova alternativa associada a uma atividade.")
            .AddEndpointFilter<ValidationFilter<CreateActivityOptionDto>>()
            .Produces<Created<HateoasResponseActivityOptionDto>>(StatusCodes.Status201Created)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", UpdateActivityOption)
            .WithName("UpdateActivityOption")
            .WithSummary("Atualiza uma alternativa")
            .WithDescription("Atualiza os dados de uma alternativa existente identificada pelo ID.")
            .AddEndpointFilter<ValidationFilter<CreateActivityOptionDto>>()
            .Produces<Ok<HateoasResponseActivityOptionDto>>(StatusCodes.Status200OK)
            .Produces<NotFound>(StatusCodes.Status404NotFound)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", DeleteActivityOption)
            .WithName("DeleteActivityOption")
            .WithSummary("Exclui uma alternativa")
            .WithDescription("Remove permanentemente a alternativa identificada pelo ID.")
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<NotFound>(StatusCodes.Status404NotFound);
    }

    static async Task<Ok<PageResponse<ResponseActivityOptionDto>>> GetAllActivityOptions(
        [FromServices] AppDbContext db,
        [FromServices] PaginationService paginator,
        HttpContext ctx,
        int page = 1,
        int size = 20)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = db.ActivityOptions
            .OrderBy(r => r.ActivityOptionId).Select(r => ResponseActivityOptionDto.From(r));

        var result = await paginator.CreatePagedResultAsync(query, page, size);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        var links = PaginationHateoas.BuildPaginationLinks("GetAllActivityOptions", page, size, result.TotalPages, ctx);
        links.Add(gen.Build("SearchActivityOptions", null, "search", "GET", ctx));
        links.Add(gen.Build("CreateActivityOption", null, "create", "POST", ctx));

        result.Links = links;
        return TypedResults.Ok(result);
    }

    static async Task<Ok<PageResponse<ResponseActivityOptionDto>>> SearchActivityOptions(
        [FromServices] AppDbContext db,
        [FromServices] PaginationService paginator,
        HttpContext ctx,
        int? activity_option,
        int page = 1,
        int size = 20)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = db.ActivityOptions
            .Where(r => r.ActivityId == activity_option)
            .OrderBy(r => r.ActivityOptionId)
            .Select(r => ResponseActivityOptionDto.From(r));

        var result = await paginator.CreatePagedResultAsync(query, page, size);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        var links = PaginationHateoas.BuildPaginationLinks("SearchActivityOptions", page, size, result.TotalPages, ctx);
        links.Add(gen.Build("GetAllActivityOptions", null, "all", "GET", ctx));
        links.Add(gen.Build("CreateActivityOption", null, "create", "POST", ctx));

        result.Links = links;
        return TypedResults.Ok(result);
    }

    static async Task<Results<Ok<HateoasResponseActivityOptionDto>, NotFound>> GetActivityOptionById(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id)
    {
        var result = await db.ActivityOptions.FindAsync(id);
        if (result is null) return TypedResults.NotFound();

        var dto = HateoasResponseActivityOptionDto.From(result);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        dto.Links = [
            gen.Build("GetActivityOptionById", new { id = result.ActivityOptionId }, "self", "GET", ctx),
            gen.Build("GetAllActivityOptions", null, "all", "GET", ctx),
            gen.Build("SearchActivityOptions", null, "search", "GET", ctx),
            gen.Build("CreateActivityOption", null, "create", "POST", ctx),
            gen.Build("UpdateActivityOption", new { id = result.ActivityOptionId }, "update", "PUT", ctx),
            gen.Build("DeleteActivityOption", new { id = result.ActivityOptionId }, "delete", "DELETE", ctx)
        ];

        return TypedResults.Ok(dto);
    }

    static async Task<Results<Created<HateoasResponseActivityOptionDto>, BadRequest>> CreateActivityOption(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        CreateActivityOptionDto requestBody)
    {
        ActivityOption activityOption = requestBody.ToEntity();

        try
        {
            await db.ActivityOptions.AddAsync(activityOption);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return TypedResults.BadRequest();
        }

        var response = HateoasResponseActivityOptionDto.From(activityOption);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        response.Links = [
            gen.Build("GetActivityOptionById", new { id = activityOption.ActivityOptionId }, "self", "GET", ctx),
            gen.Build("UpdateActivityOption", new { id = activityOption.ActivityOptionId }, "update", "PUT", ctx),
            gen.Build("DeleteActivityOption", new { id = activityOption.ActivityOptionId }, "delete", "DELETE", ctx),
            gen.Build("GetAllActivityOptions", null, "all", "GET", ctx),
            gen.Build("SearchActivityOptions", null, "search", "GET", ctx),
            gen.Build("CreateActivityOption", null, "create", "POST", ctx)
        ];

        return TypedResults.Created(
            $"/api/activityOptions/{activityOption.ActivityOptionId}",
            response
        );
    }

    static async Task<Results<Ok<HateoasResponseActivityOptionDto>, NotFound, BadRequest>> UpdateActivityOption(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id,
        CreateActivityOptionDto requestBody)
    {
        var entity = await db.ActivityOptions.FindAsync(id);
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

        var response = HateoasResponseActivityOptionDto.From(entity);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        response.Links = [
            gen.Build("GetActivityOptionById", new { id = entity.ActivityOptionId }, "self", "GET", ctx),
            gen.Build("UpdateActivityOption", new { id = entity.ActivityOptionId }, "update", "PUT", ctx),
            gen.Build("DeleteActivityOption", new { id = entity.ActivityOptionId }, "delete", "DELETE", ctx),
            gen.Build("GetAllActivityOptions", null, "all", "GET", ctx),
            gen.Build("SearchActivityOptions", null, "search", "GET", ctx),
            gen.Build("CreateActivityOption", null, "create", "POST", ctx)
        ];

        return TypedResults.Ok(response);
    }

    static async Task<Results<NoContent, NotFound>> DeleteActivityOption(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id)
    {
        var result = await db.ActivityOptions.FindAsync(id);
        if (result is null) return TypedResults.NotFound();

        db.ActivityOptions.Remove(result);
        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

}