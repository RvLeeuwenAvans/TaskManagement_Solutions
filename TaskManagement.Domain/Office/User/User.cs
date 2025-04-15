using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TaskManagement.Domain.Office.User;

// virtual members are used by entityFramework to lazy-load relationships the entities.
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class User
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(50)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public required string LastName { get; set; }
    
    // Parent Foreign key attributes:
    [Required]
    public required Guid OfficeId { get; init; }

    [ForeignKey("OfficeId")]
    public virtual required Office Office { get; set; }
    
    // Child Foreign key attribute.
    public virtual ICollection<Task.UserTask> Tasks { get; set; } = new List<Task.UserTask>();
}