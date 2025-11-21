using DotNetEnv;
using ChameleonFutureAcademyAdminApi.Endpoints;
using ChameleonFutureAcademyAdminApi.Services;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
});

builder.Services.RegisterDatabaseService(builder.Configuration);
builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<HateoasService>();
builder.Services.AddScoped<PaginationService>();

var app = builder.Build();

app.MapCoursesEndpoints();
app.MapTagsEndpoints();
app.MapContentsEndpoints();
app.MapLessonsEndpoints();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.Title = "Chameleon Future Academy Admin API";
    });
}

app.Run();