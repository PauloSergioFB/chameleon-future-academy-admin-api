using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ChameleonFutureAcademyAdminApi.Models;

[Table("cfa_tag")]
[Index(nameof(Description), IsUnique = true)]
public class Tag
{

    [Key]
    [Column("tag_id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TagId { get; set; }

    [Required]
    [Column("description")]
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;

    public ICollection<CourseTag> CourseTags { get; set; } = [];

}