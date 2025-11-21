using System.ComponentModel.DataAnnotations;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Contents;

public record CreateContentDto
{

    [Required(ErrorMessage = "O ID do curso é obrigatório.")]
    public int CourseId { get; init; }

    [Required(ErrorMessage = "O tipo é obrigatório.")]
    [MinLength(1, ErrorMessage = "O tipo não pode estar vazio.")]
    [MaxLength(20, ErrorMessage = "O tipo pode ter no máximo 20 caracteres.")]
    public string Type { get; init; } = string.Empty;

    [Required(ErrorMessage = "A posição é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A posição deve ser maior que zero.")]
    public int Position { get; init; }

    public Content ToEntity() => new()
    {
        CourseId = CourseId,
        Type = Type,
        Position = Position
    };

    public void ApplyToEntity(Content content)
    {
        content.CourseId = CourseId;
        content.Type = Type;
        content.Position = Position;
    }
}