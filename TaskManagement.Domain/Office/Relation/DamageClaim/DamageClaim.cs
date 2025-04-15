using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TaskManagement.Domain.Office.Relation.DamageClaim;

[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class DamageClaim
{
    [Key] 
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(50)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public required int DamageNumber { get; init; }
    
    [Required]
    [MaxLength(50)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public required int DamageNumberSub { get; init; }
    
    // Parent Foreign key attributes:
    [Required]
    public Guid RelationId { get; init; }
    
    [ForeignKey("RelationId")]
    public virtual required Relation Relation { get; init; }

}