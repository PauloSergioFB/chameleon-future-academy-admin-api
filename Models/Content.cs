using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChameleonFutureAcademyAdminApi.Models;

[Table("cfa_content")]
[Index(nameof(CourseId), nameof(Position), IsUnique = true)]
public class Content
{

    [Key]
    [Column("content_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ContentId { get; set; }

    [Required]
    [Column("course_id")]
    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;

    [Required]
    [Column("type")]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [Required]
    [Column("position")]
    public int Position { get; set; }

}