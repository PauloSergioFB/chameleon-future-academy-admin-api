using System.ComponentModel.DataAnnotations;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Tags;

public record CreateTagDto(

    [Required(ErrorMessage = "A descrição da tag é obrigatória.")]
    [MaxLength(100, ErrorMessage = "A descrição pode ter no máximo 100 caracteres.")]
    string Description

)
{
    public Tag ToEntity() => new()
    {
        Description = Description
    };
}