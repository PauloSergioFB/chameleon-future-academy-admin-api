using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChameleonFutureAcademyAdminApi.Models;

[Table("cfa_lesson")]
[Index(nameof(ContentId), IsUnique = true)]
public class Lesson
{

    [Key]
    [Column("lesson_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LessonId { get; set; }

    [Required]
    [Column("content_id")]
    public int ContentId { get; set; }

    public Content Content { get; set; } = null!;

    [Required]
    [Column("title")]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Column("body", TypeName = "CLOB")]
    public string Body { get; set; } = string.Empty;

    [Required]
    [Column("created_at", TypeName = "TIMESTAMP")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}