using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChameleonFutureAcademyAdminApi.Models;

[Table("CFA_ACTIVITY_OPTION")]
[Index(nameof(ActivityId), nameof(Label), IsUnique = true)]
[Index(nameof(ActivityId), nameof(Description), IsUnique = true)]
public class ActivityOption
{

    [Key]
    [Column("ACTIVITY_OPTION_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ActivityOptionId { get; set; }

    [Required]
    [Column("ACTIVITY_ID")]
    public int ActivityId { get; set; }

    public Activity Activity { get; set; } = null!;

    [Required]
    [Column("LABEL")]
    [MaxLength(1)]
    public string Label { get; set; } = string.Empty;

    [Required]
    [Column("DESCRIPTION")]
    [MaxLength(150)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column("IS_CORRECT")]
    public bool IsCorrect { get; set; }

}