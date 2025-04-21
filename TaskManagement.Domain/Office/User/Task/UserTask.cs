using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TaskManagement.Domain.Office.User.Task;

// virtual members are used by entityFramework to lazy-load relationships the entities.
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class UserTask
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [MaxLength(50)]
    [Required(ErrorMessage = "Task needs a title")]
    public required string Title { get; set; }
    
    [MaxLength(255)]
    public string? Description { get; set; }
    
    // Parent Foreign key attributes:
    [Required(ErrorMessage = "Task needs to be assigned to a user")]
    public required Guid UserId { get; set; }
    
    [ForeignKey("UserId")]
    public virtual required User User { get; set; }
    
    // Child Foreign key attributes:
    public virtual ICollection<Note.Note> Notes { get; set; } = new List<Note.Note>();
    
    public Guid? LinkedObjectId { get; set; }
    
    [ForeignKey("LinkedObjectId")]
    public virtual LinkedObject.LinkedObject? LinkedObject { get; set; }

}