using System.ComponentModel.DataAnnotations;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Badges;

public record CreateBadgeDto
{

    [Required(ErrorMessage = "O ID do curso é obrigatório.")]
    public int CourseId { get; init; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [MinLength(1, ErrorMessage = "O título não pode estar vazio.")]
    [MaxLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
    public string Title { get; init; } = string.Empty;

    [MaxLength(100, ErrorMessage = "A URL do ícone pode ter no máximo 100 caracteres.")]
    public string IconUrl { get; init; } = string.Empty;

    public Badge ToEntity() => new()
    {
        CourseId = CourseId,
        Title = Title,
        IconUrl = IconUrl
    };

    public void ApplyToEntity(Badge badge)
    {
        badge.CourseId = CourseId;
        badge.Title = Title;
        badge.IconUrl = IconUrl;
    }
}