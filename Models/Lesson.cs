using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChameleonFutureAcademyAdminApi.Models;

[Table("CFA_LESSON")]
[Index(nameof(ContentId), IsUnique = true)]
public class Lesson
{

    [Key]
    [Column("LESSON_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LessonId { get; set; }

    [Required]
    [Column("CONTENT_ID")]
    public int ContentId { get; set; }

    public Content Content { get; set; } = null!;

    [Required]
    [Column("TITLE")]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Column("BODY", TypeName = "CLOB")]
    public string Body { get; set; } = string.Empty;

    [Required]
    [Column("CREATED_AT", TypeName = "TIMESTAMP")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}