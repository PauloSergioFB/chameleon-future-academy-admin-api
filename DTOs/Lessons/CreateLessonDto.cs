using System.ComponentModel.DataAnnotations;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Lessons;

public record CreateLessonDto
{

    [Required(ErrorMessage = "O ID do conteúdo é obrigatório.")]
    public int ContentId { get; init; }

    [Required(ErrorMessage = "O título é obrigatório.")]
    [MinLength(1, ErrorMessage = "O título não pode estar vazio.")]
    [MaxLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
    public string Title { get; init; } = string.Empty;

    [Required(ErrorMessage = "O corpo da lição é obrigatório.")]
    [MinLength(1, ErrorMessage = "O corpo da lição não pode estar vazio.")]
    public string Body { get; init; } = string.Empty;

    public Lesson ToEntity() => new()
    {
        ContentId = ContentId,
        Title = Title,
        Body = Body
    };

    public void ApplyToEntity(Lesson lesson)
    {
        lesson.ContentId = ContentId;
        lesson.Title = Title;
        lesson.Body = Body;
    }
}