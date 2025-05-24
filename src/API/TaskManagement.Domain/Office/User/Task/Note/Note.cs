using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TaskManagement.Domain.Office.User.Task.Note;

// virtual members are used by entityFramework to lazy-load relationships the entities.
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class Note
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [MaxLength(255)]
    [Required(ErrorMessage = "Note can't be empty")]
    public required string Content { get; set; }
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    // Parent Foreign key attributes:
    public Guid TaskId { get; init; }

    [ForeignKey("TaskId")]
    public virtual required UserTask UserTask { get; init; }

}