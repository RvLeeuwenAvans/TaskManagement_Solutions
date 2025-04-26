using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Domain.Office.Relation.DamageClaim;

[Index(nameof(DamageNumber), IsUnique = true)]
[Index(nameof(DamageNumberSub), IsUnique = true)]
// virtual members are used by entityFramework to lazy-load relationships the entities.
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class DamageClaim
{
    [Key] 
    public Guid Id { get; init; } = Guid.NewGuid();
    
    // non mutable; generated in database; to simulate existing logic.
    [MaxLength(50)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DamageNumber { get;  private set; }
    
    // non mutable; generated in database; to simulate existing logic.
    [MaxLength(50)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DamageNumberSub { get; private set; }
    
    [Required]
    [MaxLength(100)]
    public required string Type { get; set; }
    
    // Parent Foreign key attributes:
    [Required]
    public Guid RelationId { get; init; }
    
    [ForeignKey("RelationId")]
    public virtual required Relation Relation { get; init; }

}