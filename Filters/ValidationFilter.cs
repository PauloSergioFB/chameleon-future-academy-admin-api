using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ChameleonFutureAcademyAdminApi.Filters;

public partial class ValidationFilter<T> : IEndpointFilter
{

    [GeneratedRegex("(?<!^)([A-Z])")]
    private static partial Regex SnakeCaseRegex();

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return SnakeCaseRegex().Replace(input, "_$1").ToLower();
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var dto = context.Arguments.OfType<T>().FirstOrDefault();
        if (dto is not null)
        {
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, new ValidationContext(dto), results, true);

            if (!isValid)
            {
                var errors = results.ToDictionary(
                    r => ToSnakeCase(r.MemberNames.FirstOrDefault() ?? "error"),
                    r => r.ErrorMessage ?? "Valor inválido."
                    );

                return Results.BadRequest(errors);
            }
        }

        return await next(context);
    }

}