using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChameleonFutureAcademyAdminApi.Models;

[Table("CFA_CONTENT")]
[Index(nameof(CourseId), nameof(Position), IsUnique = true)]
public class Content
{

    [Key]
    [Column("CONTENT_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ContentId { get; set; }

    [Required]
    [Column("COURSE_ID")]
    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;

    [Required]
    [Column("TYPE")]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [Required]
    [Column("POSITION")]
    public int Position { get; set; }

}