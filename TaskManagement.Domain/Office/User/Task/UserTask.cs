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
    
    public required DateTime DueDate { get; set; }
    
    [MaxLength(50)]
    public required string Title { get; set; }
    
    [MaxLength(255)]
    public string? Description { get; set; }
    
    [Required]
    [MaxLength(50)]
    // not a FK, cause a the creator of a task might not exist anymore. And Only the name is relevant.
    public required string CreatorName { get; set; }
    
    // Parent Foreign key attributes:
    public required Guid UserId { get; set; }
    
    [ForeignKey("UserId")]
    public virtual required User User { get; set; }
    
    // Child Foreign key attributes:
    public virtual ICollection<Note.Note> Notes { get; set; } = new List<Note.Note>();
    
    public Guid? LinkedObjectId { get; set; }
    
    [ForeignKey("LinkedObjectId")]
    public virtual LinkedObject.LinkedObject? LinkedObject { get; set; }

}