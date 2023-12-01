using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApi.Entities;

[Table("Post")]
public class Post
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    [Required]
    [MaxLength(4000)]
    public string Content { get; set; }

    [Required]
    public DateTime PostDate { get; set; }
    
    public User User { get; set; }
}