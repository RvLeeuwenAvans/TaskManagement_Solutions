using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore; 

namespace TaskManagement.Domain.Office;

[Index(nameof(OfficeCode), IsUnique = true)]
// virtual members are used by entityFramework to lazy-load relationships the entities.
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class Office
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    
    // non mutable; generated in database; to simulate existing logic.
    [Required]
    [MaxLength(50)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int OfficeCode { get; init; }
        
    // Child Foreign key attributes:
    public virtual ICollection<User.User> Users { get; set; } = new List<User.User>();
    public virtual ICollection<Relation.Relation> Relations { get; set; } = new List<Relation.Relation>();
}