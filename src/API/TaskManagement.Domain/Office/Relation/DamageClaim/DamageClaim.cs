using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Office.User.Task.LinkedObject;

namespace TaskManagement.Domain.Office.Relation.DamageClaim;

[Index(nameof(DamageNumber), IsUnique = true)]
// virtual members are used by entityFramework to lazy-load relationships the entities.
[SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
public class DamageClaim
{
    [Key] public Guid Id { get; init; } = Guid.NewGuid();

    // non mutable; generated in database; to simulate existing logic.
    [MaxLength(50)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int DamageNumber { get; private set; } = new Random().Next(1, 1000);

    // non mutable; generated in database; to simulate existing logic.
    [MaxLength(50)]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int DamageNumberSub { get; set; } = new Random().Next(1, 1000);

    [Required] [MaxLength(100)] public required string Type { get; set; }

    // Parent Foreign key attributes:
    [Required] public Guid RelationId { get; init; }

    [ForeignKey("RelationId")] public virtual required Relation Relation { get; init; }

    public virtual ICollection<LinkedObject> LinkedObjects { get; set; } = new List<LinkedObject>();
}