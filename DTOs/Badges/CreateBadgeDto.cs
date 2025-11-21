using System.ComponentModel.DataAnnotations;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Badges;

public record CreateBadgeDto(

    [Required(ErrorMessage = "O ID do curso é obrigatório.")]
    int CourseId,

    [Required(ErrorMessage = "O título é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
    string Title,

    [MaxLength(100, ErrorMessage = "A URL do ícone pode ter no máximo 100 caracteres.")]
    string? IconUrl

)
{
    public Badge ToEntity() => new()
    {
        CourseId = CourseId,
        Title = Title,
        IconUrl = IconUrl ?? string.Empty
    };
}