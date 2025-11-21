using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChameleonFutureAcademyAdminApi.Models;

[Table("cfa_user_account")]
[Index(nameof(Title), IsUnique = true)]
public class Course
{

    [Key]
    [Column("course_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CourseId { get; set; }

    [Required]
    [Column("title")]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;

    [Column("author")]
    [MaxLength(100)]
    public string Author { get; set; } = string.Empty;

    [Column("thumbnail_url")]
    [MaxLength(100)]
    public string ThumbnailUrl { get; set; } = string.Empty;

    [Required]
    [Column("created_at", TypeName = "TIMESTAMP")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<CourseTag> CourseTags { get; set; } = [];

}