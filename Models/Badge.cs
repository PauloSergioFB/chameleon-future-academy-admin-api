using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChameleonFutureAcademyAdminApi.Models;

[Table("CFA_BADGE")]
[Index(nameof(Title), IsUnique = true)]
public class Badge
{

    [Key]
    [Column("BADGE_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BadgeId { get; set; }

    [Required]
    [Column("COURSE_ID")]
    public int CourseId { get; set; }

    public Course Course { get; set; } = null!;

    [Required]
    [Column("TITLE")]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Column("ICON_URL")]
    [MaxLength(100)]
    public string IconUrl { get; set; } = string.Empty;

}