using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChameleonFutureAcademyAdminApi.Models;

[Table("CFA_COURSE")]
[Index(nameof(Title), IsUnique = true)]
public class Course
{

    [Key]
    [Column("COURSE_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CourseId { get; set; }

    [Required]
    [Column("TITLE")]
    [MaxLength(100)]
    public string? Title { get; set; }

    [Column("DESCRIPTION")]
    [MaxLength(255)]
    public string? Description { get; set; }

    [Column("AUTHOR")]
    [MaxLength(100)]
    public string? Author { get; set; }

    [Column("THUMBNAIL_URL")]
    [MaxLength(100)]
    public string? ThumbnailUrl { get; set; }

    [Required]
    [Column("CREATED_AT", TypeName = "TIMESTAMP")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<CourseTag> CourseTags { get; set; } = [];

}