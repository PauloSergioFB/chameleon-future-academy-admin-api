using System.ComponentModel.DataAnnotations;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Activities;

public record CreateActivityDto(

    [Required(ErrorMessage = "O ID do conteúdo é obrigatório.")]
    int ContentId,

    [Required(ErrorMessage = "O título é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
    string Title,

    [Required(ErrorMessage = "O corpo da atividade é obrigatório.")]
    string Body,

    [Required(ErrorMessage = "A explicação é obrigatória.")]
    [MaxLength(500, ErrorMessage = "A explicação pode ter no máximo 500 caracteres.")]
    string Explanation

)
{
    public Activity ToEntity() => new()
    {
        ContentId = ContentId,
        Title = Title,
        Body = Body,
        Explanation = Explanation
    };
}