using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChameleonFutureAcademyAdminApi.Models;

[Table("cfa_badge")]
[Index(nameof(Title), IsUnique = true)]
public class Badge
{

    [Key]
    [Column("badge_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BadgeId { get; set; }

    [Required]
    [Column("course_id")]
    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;

    [Required]
    [Column("title")]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Column("icon_url")]
    [MaxLength(100)]
    public string IconUrl { get; set; } = string.Empty;

}