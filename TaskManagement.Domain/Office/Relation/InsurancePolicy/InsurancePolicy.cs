using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TaskManagement.Domain.Office.Relation.InsurancePolicy;

[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class InsurancePolicy
{
    [Key] public Guid Id { get; init; } = Guid.NewGuid();
    
    // non mutable; generated in database; to simulate existing logic.
    [Required]
    [MaxLength(50)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public int PolicyNumber { get; init; }
    
    [Required]
    [MaxLength(100)]
    public required string Type { get; set; }
    
    // Parent Foreign key attributes:
    [Required]
    public Guid RelationId { get; init; }
    
    [ForeignKey("RelationId")]
    public virtual required Relation Relation { get; init; }

}