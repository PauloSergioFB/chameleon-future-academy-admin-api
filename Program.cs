using DotNetEnv;
using ChameleonFutureAcademyAdminApi.Services;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using ChameleonFutureAcademyAdminApi.Filters;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.RegisterDatabaseService(builder.Configuration);
builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<ValidationFilter>();
builder.Services.AddSingleton<HateoasService>();

var app = builder.Build();

var api = app.MapGroup("/api");

api.AddEndpointFilter<ValidationFilter>();

app.MapOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.Title = "Chameleon Future Academy Admin API";
    });
}

app.Run();