using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChameleonFutureAcademyAdminApi.Models;

[Table("CFA_TAG")]
[Index(nameof(Description), IsUnique = true)]
public class Tag
{

    [Key]
    [Column("TAG_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TagId { get; set; }

    [Required]
    [Column("DESCRIPTION")]
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;

    public ICollection<CourseTag> CourseTags { get; set; } = [];

}