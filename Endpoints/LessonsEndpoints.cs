using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ChameleonFutureAcademyAdminApi.Data;
using ChameleonFutureAcademyAdminApi.DTOs.Lessons;
using ChameleonFutureAcademyAdminApi.DTOs.Pagination;
using ChameleonFutureAcademyAdminApi.Models;
using ChameleonFutureAcademyAdminApi.Services;
using ChameleonFutureAcademyAdminApi.Hateoas;
using Microsoft.AspNetCore.Mvc;
using ChameleonFutureAcademyAdminApi.Filters;

namespace ChameleonFutureAcademyAdminApi.Endpoints;

public static class LessonsEndpoints
{

    public static void MapLessonsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/lessons").WithTags("Aulas");

        group.MapGet("/", GetAllLessons)
            .WithName("GetAllLessons")
            .WithSummary("Lista aulas")
            .WithDescription("Retorna todas as aulas cadastradas, com suporte a paginação.")
            .Produces<Ok<PageResponse<ResponseLessonDto>>>(StatusCodes.Status200OK);

        group.MapGet("/search", SearchLessons)
            .WithName("SearchLessons")
            .WithSummary("Busca aulas")
            .WithDescription("Busca aulas filtrando pelo ID do conteúdo ao qual pertencem.")
            .Produces<Ok<PageResponse<ResponseLessonDto>>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", GetLessonById)
            .WithName("GetLessonById")
            .WithSummary("Obtém uma aula específica")
            .WithDescription("Retorna as informações de uma aula identificada pelo ID.")
            .Produces<Ok<HateoasResponseLessonDto>>(StatusCodes.Status200OK)
            .Produces<NotFound>(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateLesson)
            .WithName("CreateLesson")
            .WithSummary("Cria uma aula")
            .WithDescription("Cria uma nova aula vinculada a um conteúdo.")
            .AddEndpointFilter<ValidationFilter<CreateLessonDto>>()
            .Produces<Created<HateoasResponseLessonDto>>(StatusCodes.Status201Created)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", UpdateLesson)
            .WithName("UpdateLesson")
            .WithSummary("Atualiza uma aula")
            .WithDescription("Atualiza os dados de uma aula existente identificada pelo ID.")
            .AddEndpointFilter<ValidationFilter<CreateLessonDto>>()
            .Produces<Ok<HateoasResponseLessonDto>>(StatusCodes.Status200OK)
            .Produces<NotFound>(StatusCodes.Status404NotFound)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", DeleteLesson)
            .WithName("DeleteLesson")
            .WithSummary("Exclui uma aula")
            .WithDescription("Remove permanentemente a aula identificada pelo ID.")
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<NotFound>(StatusCodes.Status404NotFound);
    }

    static async Task<Ok<PageResponse<ResponseLessonDto>>> GetAllLessons(
        [FromServices] AppDbContext db,
        [FromServices] PaginationService paginator,
        HttpContext ctx,
        int page = 1,
        int size = 20)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = db.Lessons
            .OrderBy(r => r.LessonId).Select(r => ResponseLessonDto.From(r));

        var result = await paginator.CreatePagedResultAsync(query, page, size);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        var links = PaginationHateoas.BuildPaginationLinks("GetAllLessons", page, size, result.TotalPages, ctx);
        links.Add(gen.Build("SearchLessons", null, "search", "GET", ctx));
        links.Add(gen.Build("CreateLesson", null, "create", "POST", ctx));

        result.Links = links;
        return TypedResults.Ok(result);
    }

    static async Task<Ok<PageResponse<ResponseLessonDto>>> SearchLessons(
        [FromServices] AppDbContext db,
        [FromServices] PaginationService paginator,
        HttpContext ctx,
        int content_id,
        int page = 1,
        int size = 20)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = db.Lessons
            .Where(r => r.ContentId == content_id)
            .OrderBy(r => r.LessonId)
            .Select(r => ResponseLessonDto.From(r));

        var result = await paginator.CreatePagedResultAsync(query, page, size);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        var links = PaginationHateoas.BuildPaginationLinks("SearchLessons", page, size, result.TotalPages, ctx);
        links.Add(gen.Build("GetAllLessons", null, "all", "GET", ctx));
        links.Add(gen.Build("CreateLesson", null, "create", "POST", ctx));

        result.Links = links;
        return TypedResults.Ok(result);
    }

    static async Task<Results<Ok<HateoasResponseLessonDto>, NotFound>> GetLessonById(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id)
    {
        var result = await db.Lessons.FindAsync(id);
        if (result is null) return TypedResults.NotFound();

        var dto = HateoasResponseLessonDto.From(result);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        dto.Links = [
            gen.Build("GetLessonById", new { id = result.LessonId }, "self", "GET", ctx),
            gen.Build("GetAllLessons", null, "all", "GET", ctx),
            gen.Build("SearchLessons", null, "search", "GET", ctx),
            gen.Build("CreateLesson", null, "create", "POST", ctx),
            gen.Build("UpdateLesson", new { id = result.LessonId }, "update", "PUT", ctx),
            gen.Build("DeleteLesson", new { id = result.LessonId }, "delete", "DELETE", ctx)
        ];

        return TypedResults.Ok(dto);
    }

    static async Task<Results<Created<HateoasResponseLessonDto>, BadRequest>> CreateLesson(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        CreateLessonDto requestBody)
    {
        Lesson lesson = requestBody.ToEntity();

        try
        {
            await db.Lessons.AddAsync(lesson);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return TypedResults.BadRequest();
        }

        var response = HateoasResponseLessonDto.From(lesson);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        response.Links = [
            gen.Build("GetLessonById", new { id = lesson.LessonId }, "self", "GET", ctx),
            gen.Build("UpdateLesson", new { id = lesson.LessonId }, "update", "PUT", ctx),
            gen.Build("DeleteLesson", new { id = lesson.LessonId }, "delete", "DELETE", ctx),
            gen.Build("GetAllLessons", null, "all", "GET", ctx),
            gen.Build("SearchLessons", null, "search", "GET", ctx),
            gen.Build("CreateLesson", null, "create", "POST", ctx)
        ];

        return TypedResults.Created(
            $"/api/lessons/{lesson.LessonId}",
            response
        );
    }

    static async Task<Results<Ok<HateoasResponseLessonDto>, NotFound, BadRequest>> UpdateLesson(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id,
        CreateLessonDto requestBody)
    {
        var entity = await db.Lessons.FindAsync(id);
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

        var response = HateoasResponseLessonDto.From(entity);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        response.Links = [
            gen.Build("GetLessonById", new { id = entity.LessonId }, "self", "GET", ctx),
            gen.Build("UpdateLesson", new { id = entity.LessonId }, "update", "PUT", ctx),
            gen.Build("DeleteLesson", new { id = entity.LessonId }, "delete", "DELETE", ctx),
            gen.Build("GetAllLessons", null, "all", "GET", ctx),
            gen.Build("SearchLessons", null, "search", "GET", ctx),
            gen.Build("CreateLesson", null, "create", "POST", ctx)
        ];

        return TypedResults.Ok(response);
    }

    static async Task<Results<NoContent, NotFound>> DeleteLesson(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id)
    {
        var result = await db.Lessons.FindAsync(id);
        if (result is null) return TypedResults.NotFound();

        db.Lessons.Remove(result);
        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

}