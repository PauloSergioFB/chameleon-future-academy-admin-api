using System.ComponentModel.DataAnnotations;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Contents;

public record CreateContentDto(

    [Required(ErrorMessage = "O ID do curso é obrigatório.")]
    int CourseId,

    [Required(ErrorMessage = "O tipo é obrigatório.")]
    [MaxLength(20, ErrorMessage = "O tipo pode ter no máximo 20 caracteres.")]
    string Type,

    [Required(ErrorMessage = "A posição é obrigatória.")]
    [Range(1, int.MaxValue, ErrorMessage = "A posição deve ser maior que zero.")]
    int Position

)
{
    public Content ToEntity() => new()
    {
        CourseId = CourseId,
        Type = Type,
        Position = Position
    };
}