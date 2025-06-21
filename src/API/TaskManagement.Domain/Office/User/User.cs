using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Domain.Office.User;

[Index(nameof(Email), IsUnique = true)]
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
    
    [Required]
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    // Im not gonna determine a max length in the DB cause i have no clue how long the hash is going to be.
    public required string Password { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string Email { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(10)")] 
    public UserRole Role { get; set; } = UserRole.User;
    
    // Parent Foreign key attributes:
    [Required]
    public required Guid OfficeId { get; init; }

    [ForeignKey("OfficeId")]
    public virtual required Office Office { get; set; }
    
    // Child Foreign key attribute.
    public virtual ICollection<Task.UserTask> Tasks { get; set; } = new List<Task.UserTask>();
}