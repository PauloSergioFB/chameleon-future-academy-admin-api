using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using ChameleonFutureAcademyAdminApi.Data;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection"))
);

var app = builder.Build();

app.Run();