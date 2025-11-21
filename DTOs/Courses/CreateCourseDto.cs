using System.ComponentModel.DataAnnotations;
using ChameleonFutureAcademyAdminApi.Models;

namespace ChameleonFutureAcademyAdminApi.DTOs.Courses;

public record CreateCourseDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [MinLength(1, ErrorMessage = "O título não pode estar vazio.")]
    [MaxLength(100, ErrorMessage = "O título pode ter no máximo 100 caracteres.")]
    public string Title { get; init; } = string.Empty;

    [MaxLength(255, ErrorMessage = "A descrição pode ter no máximo 255 caracteres.")]
    public string? Description { get; init; }

    [MaxLength(100, ErrorMessage = "O nome do autor pode ter no máximo 100 caracteres.")]
    public string? Author { get; init; }

    [MaxLength(100, ErrorMessage = "A thumbnail pode ter no máximo 100 caracteres.")]
    public string? ThumbnailUrl { get; init; }

    public Course ToEntity() => new()
    {
        Title = Title,
        Description = Description,
        Author = Author,
        ThumbnailUrl = ThumbnailUrl
    };

    public void ApplyToEntity(Course course)
    {
        course.Title = Title;
        course.Description = Description;
        course.Author = Author;
        course.ThumbnailUrl = ThumbnailUrl;
    }
}