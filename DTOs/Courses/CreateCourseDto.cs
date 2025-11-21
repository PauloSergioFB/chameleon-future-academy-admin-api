using System.ComponentModel.DataAnnotations;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Courses;

public record CreateCourseDto(

    [Required(ErrorMessage = "O título é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
    string Title,

    [MaxLength(255, ErrorMessage = "A descrição pode ter no máximo 255 caracteres.")]
    string Description,

    [MaxLength(100, ErrorMessage = "O nome do autor pode ter no máximo 100 caracteres.")]
    string Author,

    [MaxLength(100, ErrorMessage = "A URL da thumbnail pode ter no máximo 100 caracteres.")]
    string ThumbnailUrl

)
{
    public Course ToEntity() => new()
    {
        Title = Title,
        Description = Description,
        Author = Author,
        ThumbnailUrl = ThumbnailUrl
    };
}