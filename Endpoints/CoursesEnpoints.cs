using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ChameleonFutureAcademyAdminApi.Data;
using ChameleonFutureAcademyAdminApi.DTOs.Courses;
using ChameleonFutureAcademyAdminApi.DTOs.Pagination;
using ChameleonFutureAcademyAdminApi.Models;
using ChameleonFutureAcademyAdminApi.Services;
using ChameleonFutureAcademyAdminApi.Hateoas;
using Microsoft.AspNetCore.Mvc;
using ChameleonFutureAcademyAdminApi.Filters;

namespace ChameleonFutureAcademyAdminApi.Endpoints;

public static class CoursesEndpoints
{

    public static void MapCoursesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/courses").WithTags("Cursos");

        group.MapGet("/", GetAllCourses)
            .WithName("GetAllCourses")
            .WithSummary("Lista cursos")
            .WithDescription("Retorna todos os cursos.")
            .Produces<Ok<PageResponse<ResponseCourseDto>>>(StatusCodes.Status200OK);

        group.MapGet("/search", SearchCourses)
            .WithName("SearchCourses")
            .WithSummary("Busca cursos")
            .WithDescription("Realiza uma busca de cursos filtrando pelo titulo informado.")
            .Produces<Ok<PageResponse<ResponseCourseDto>>>(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", GetCourseById)
            .WithName("GetCourseById")
            .WithSummary("Obtém um curso especifico")
            .WithDescription("Retorna as informações de um curso específico identificado pelo ID.")
            .Produces<Ok<HateoasResponseCourseDto>>(StatusCodes.Status200OK)
            .Produces<NotFound>(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateCourse)
            .WithName("CreateCourse")
            .WithSummary("Cria um curso")
            .WithDescription("Cria um novo curso com os dados fornecidos.")
            .AddEndpointFilter<ValidationFilter<CreateCourseDto>>()
            .Produces<Created<HateoasResponseCourseDto>>(StatusCodes.Status201Created)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", UpdateCourse)
            .WithName("UpdateCourse")
            .WithSummary("Atualiza um curso")
            .WithDescription("Atualiza os dados de um curso existente identificado pelo ID.")
            .AddEndpointFilter<ValidationFilter<CreateCourseDto>>()
            .Produces<Ok<HateoasResponseCourseDto>>(StatusCodes.Status200OK)
            .Produces<NotFound>(StatusCodes.Status404NotFound)
            .Produces<BadRequest>(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", DeleteCourse)
            .WithName("DeleteCourse")
            .WithSummary("Exclui um curso")
            .WithDescription("Exclui permanentemente o curso identificado pelo ID.")
            .Produces<NoContent>(StatusCodes.Status204NoContent)
            .Produces<NotFound>(StatusCodes.Status404NotFound);
    }

    static async Task<Ok<PageResponse<ResponseCourseDto>>> GetAllCourses(
        [FromServices] AppDbContext db,
        [FromServices] PaginationService paginator,
        HttpContext ctx,
        int page = 1,
        int size = 20)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = db.Courses
            .OrderBy(c => c.CourseId).Select(c => ResponseCourseDto.From(c));

        var result = await paginator.CreatePagedResultAsync(query, page, size);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        var links = PaginationHateoas.BuildPaginationLinks("GetAllCourses", page, size, result.TotalPages, ctx);
        links.Add(gen.Build("SearchCourses", null, "search", "GET", ctx));
        links.Add(gen.Build("CreateCourse", null, "create", "POST", ctx));

        result.Links = links;
        return TypedResults.Ok(result);
    }

    static async Task<Ok<PageResponse<ResponseCourseDto>>> SearchCourses(
        [FromServices] AppDbContext db,
        [FromServices] PaginationService paginator,
        HttpContext ctx,
        string title = "",
        int page = 1,
        int size = 20)
    {
        if (page < 1) page = 1;
        if (size < 1) size = 20;

        var query = db.Courses
            .Where(c => EF.Functions.Like(c.Title, $"%{title}%"))
            .OrderBy(c => c.CourseId)
            .Select(c => ResponseCourseDto.From(c));

        var result = await paginator.CreatePagedResultAsync(query, page, size);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        var links = PaginationHateoas.BuildPaginationLinks("SearchCourses", page, size, result.TotalPages, ctx);
        links.Add(gen.Build("GetAllCourses", null, "all", "GET", ctx));
        links.Add(gen.Build("CreateCourse", null, "create", "POST", ctx));

        result.Links = links;
        return TypedResults.Ok(result);
    }

    static async Task<Results<Ok<HateoasResponseCourseDto>, NotFound>> GetCourseById(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id)
    {
        var result = await db.Courses.FindAsync(id);
        if (result is null) return TypedResults.NotFound();

        var dto = HateoasResponseCourseDto.From(result);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        dto.Links = [
            gen.Build("GetCourseById", new { id = result.CourseId }, "self", "GET", ctx),
            gen.Build("GetAllCourses", null, "all", "GET", ctx),
            gen.Build("SearchCourses", null, "search", "GET", ctx),
            gen.Build("CreateCourse", null, "create", "POST", ctx),
            gen.Build("UpdateCourse", new { id = result.CourseId }, "update", "PUT", ctx),
            gen.Build("DeleteCourse", new { id = result.CourseId }, "delete", "DELETE", ctx)
        ];

        return TypedResults.Ok(dto);
    }

    static async Task<Results<Created<HateoasResponseCourseDto>, BadRequest>> CreateCourse(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        CreateCourseDto requestBody)
    {
        Course course = requestBody.ToEntity();

        try
        {
            await db.Courses.AddAsync(course);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return TypedResults.BadRequest();
        }

        var response = HateoasResponseCourseDto.From(course);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        response.Links = [
            gen.Build("GetCourseById", new { id = course.CourseId }, "self", "GET", ctx),
            gen.Build("UpdateCourse", new { id = course.CourseId }, "update", "PUT", ctx),
            gen.Build("DeleteCourse", new { id = course.CourseId }, "delete", "DELETE", ctx),
            gen.Build("GetAllCourses", null, "all", "GET", ctx),
            gen.Build("SearchCourses", null, "search", "GET", ctx),
            gen.Build("CreateCourse", null, "create", "POST", ctx)
        ];

        return TypedResults.Created(
            $"/api/courses/{course.CourseId}",
            response
        );
    }

    static async Task<Results<Ok<HateoasResponseCourseDto>, NotFound, BadRequest>> UpdateCourse(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id,
        CreateCourseDto requestBody)
    {
        var entity = await db.Courses.FindAsync(id);
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

        var response = HateoasResponseCourseDto.From(entity);

        var gen = ctx.RequestServices.GetRequiredService<HateoasService>();

        response.Links = [
            gen.Build("GetCourseById", new { id = entity.CourseId }, "self", "GET", ctx),
            gen.Build("UpdateCourse", new { id = entity.CourseId }, "update", "PUT", ctx),
            gen.Build("DeleteCourse", new { id = entity.CourseId }, "delete", "DELETE", ctx),
            gen.Build("GetAllCourses", null, "all", "GET", ctx),
            gen.Build("SearchCourses", null, "search", "GET", ctx),
            gen.Build("CreateCourse", null, "create", "POST", ctx)
        ];

        return TypedResults.Ok(response);
    }

    static async Task<Results<NoContent, NotFound>> DeleteCourse(
        [FromServices] AppDbContext db,
        HttpContext ctx,
        int id)
    {
        var result = await db.Courses.FindAsync(id);
        if (result is null) return TypedResults.NotFound();

        db.Courses.Remove(result);
        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

}