using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using TaskManagement.Domain.Office.Relation.DamageClaim;
using TaskManagement.Domain.Office.Relation.InsurancePolicy;

namespace TaskManagement.Domain.Office.User.Task.LinkedObject;

// virtual members are used by entityFramework to lazy-load relationships the entities.
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class LinkedObject
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [Required]
    public Guid TaskId { get; init; } 
    
    [ForeignKey("TaskId")]
    public virtual required UserTask UserTask { get; init; }
    
    public Guid? RelationId { get; set; }
    
    [ForeignKey("RelationId")]
    public virtual Relation.Relation? Relation  { get; init; }
    
    public Guid? DamageClaimId { get; set; }
    
    [ForeignKey("DamageClaimId")]
    public virtual DamageClaim?  DamageClaim { get; init; }

    public Guid? InsurancePolicyId { get; set; }
    
    [ForeignKey("InsurancePolicyId")]
    public virtual InsurancePolicy? InsurancePolicy { get; init; }
}