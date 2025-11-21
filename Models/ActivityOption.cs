using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChameleonFutureAcademyAdminApi.Models;

[Table("cfa_activity_option")]
[Index(nameof(ActivityId), nameof(Label), IsUnique = true)]
[Index(nameof(ActivityId), nameof(Description), IsUnique = true)]
public class ActivityOption
{

    [Key]
    [Column("activity_option_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ActivityOptionId { get; set; }

    [Required]
    [Column("activity_id")]
    public int ActivityId { get; set; }

    public Activity Activity { get; set; } = null!;

    [Required]
    [Column("label")]
    [MaxLength(1)]
    public string Label { get; set; } = string.Empty;

    [Required]
    [Column("description")]
    [MaxLength(150)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column("is_correct")]
    public bool IsCorrect { get; set; }

}