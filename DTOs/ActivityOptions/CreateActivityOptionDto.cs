using System.ComponentModel.DataAnnotations;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.ActivityOptions;

public record CreateActivityOptionDto
{

    [Required(ErrorMessage = "O ID da atividade é obrigatório.")]
    public int ActivityId { get; init; }

    [Required(ErrorMessage = "O label é obrigatório.")]
    [MinLength(1, ErrorMessage = "O label não pode estar vazia.")]
    [MaxLength(1, ErrorMessage = "O label deve ter apenas 1 caractere.")]
    [RegularExpression("[abcde]", ErrorMessage = "O label deve ser uma das letras: a, b, c, d ou e.")]
    public string Label { get; init; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [MinLength(1, ErrorMessage = "A descrição não pode estar vazia.")]
    [MaxLength(150, ErrorMessage = "A descrição pode ter no máximo 150 caracteres.")]
    public string Description { get; init; } = string.Empty;

    [Required(ErrorMessage = "O campo 'IsCorrect' é obrigatório.")]
    public bool IsCorrect { get; init; }

    public ActivityOption ToEntity() => new()
    {
        ActivityId = ActivityId,
        Label = Label,
        Description = Description,
        IsCorrect = IsCorrect
    };

    public void ApplyToEntity(ActivityOption activityOption)
    {
        activityOption.ActivityId = ActivityId;
        activityOption.Label = Label;
        activityOption.Description = Description;
        activityOption.IsCorrect = IsCorrect;
    }
}