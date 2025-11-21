using Microsoft.EntityFrameworkCore;
using ChameleonFutureAcademyAdminApi.Data;

namespace ChameleonFutureAcademyAdminApi.Services;

public static class DatabaseService
{

    public static void RegisterDatabaseService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(
            options => options.UseOracle(configuration.GetConnectionString("OracleConnection"))
        );

        services.AddDatabaseDeveloperPageExceptionFilter();
    }

}