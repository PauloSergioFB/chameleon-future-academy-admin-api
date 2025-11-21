using System.ComponentModel.DataAnnotations;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.ActivityOptions;

public record CreateActivityOptionDto(

    [Required(ErrorMessage = "O ID da atividade é obrigatório.")]
    int ActivityId,

    [Required(ErrorMessage = "O label é obrigatório.")]
    [MaxLength(1, ErrorMessage = "O label deve ter apenas 1 caractere.")]
    [RegularExpression("[abcde]", ErrorMessage = "O label deve ser uma das letras: a, b, c, d ou e.")]
    string Label,

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [MaxLength(150, ErrorMessage = "A descrição pode ter no máximo 150 caracteres.")]
    string Description,

    [Required(ErrorMessage = "O campo 'IsCorrect' é obrigatório.")]
    bool IsCorrect

)
{
    public ActivityOption ToEntity() => new()
    {
        ActivityId = ActivityId,
        Label = Label,
        Description = Description,
        IsCorrect = IsCorrect
    };
}