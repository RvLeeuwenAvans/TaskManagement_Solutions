using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.Domain.Office.Relation;

[Index(nameof(RelationNumber), IsUnique = true)]
// virtual members are used by entityFramework to lazy-load relationships the entities.
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class Relation
{
    [Key] public Guid Id { get; init; } = Guid.NewGuid();

    // non mutable; generated in database; to simulate existing logic.
    [MaxLength(50)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RelationNumber { get; private set; }
    
    [Required]
    [MaxLength(50)]
    public required string FirstName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string LastName { get; set; }
    
    // Parent Foreign key attributes:
    [Required]
    public Guid OfficeId { get; init; }

    [ForeignKey("OfficeId")] public virtual required Office Office { get; init; }

    // Child Foreign key attribute.
    public virtual ICollection<InsurancePolicy.InsurancePolicy> InsurancePolicies { get; set; } =
        new List<InsurancePolicy.InsurancePolicy>();

    public virtual ICollection<DamageClaim.DamageClaim> DamageClaims { get; set; } =
        new List<DamageClaim.DamageClaim>();
}