using System.ComponentModel.DataAnnotations;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Lessons;

public record CreateLessonDto(

    [Required(ErrorMessage = "O ID do conteúdo é obrigatório.")]
    int ContentId,

    [Required(ErrorMessage = "O título é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
    string Title,

    [Required(ErrorMessage = "O corpo da lição é obrigatório.")]
    string Body

)
{
    public Lesson ToEntity() => new()
    {
        ContentId = ContentId,
        Title = Title,
        Body = Body
    };
}