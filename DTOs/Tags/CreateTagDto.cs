using System.ComponentModel.DataAnnotations;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Tags;

public record CreateTagDto
{

    [Required(ErrorMessage = "A descrição da tag é obrigatória.")]
    [MinLength(1, ErrorMessage = "A descrição não pode estar vazia.")]
    [MaxLength(100, ErrorMessage = "A descrição pode ter no máximo 100 caracteres.")]
    public string Description { get; init; } = string.Empty;

    public Tag ToEntity() => new()
    {
        Description = Description
    };

    public void ApplyToEntity(Tag tag)
    {
        tag.Description = Description;
    }
}