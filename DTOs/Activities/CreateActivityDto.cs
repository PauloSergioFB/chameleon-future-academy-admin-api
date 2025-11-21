using System.ComponentModel.DataAnnotations;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Activities;

public record CreateActivityDto(

    [Required(ErrorMessage = "O ID do conteúdo é obrigatório.")]
    int ContentId,

    [Required(ErrorMessage = "O título é obrigatório.")]
    [MinLength(1, ErrorMessage = "O título não pode estar vazio.")]
    [MaxLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
    string Title,

    [Required(ErrorMessage = "O corpo da atividade é obrigatório.")]
    [MinLength(1, ErrorMessage = "O corpo da atividade não pode estar vazio.")]
    string Body,

    [Required(ErrorMessage = "A explicação é obrigatória.")]
    [MinLength(1, ErrorMessage = "A explicação não pode estar vazia.")]
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

    public void ApplyToEntity(Activity activity)
    {
        activity.ContentId = ContentId;
        activity.Title = Title;
        activity.Body = Body;
        activity.Explanation = Explanation;
    }
}