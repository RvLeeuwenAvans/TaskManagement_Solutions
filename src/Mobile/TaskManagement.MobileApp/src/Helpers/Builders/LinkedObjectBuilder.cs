using TaskManagement.DTO.Office.Relation;
using TaskManagement.DTO.Office.Relation.DamageClaim;
using TaskManagement.DTO.Office.Relation.InsurancePolicy;
using TaskManagement.MobileApp.Models.Collections;

namespace TaskManagement.MobileApp.Helpers.Builders;

public class LinkedObjectModelBuilder
{
    private Guid _id;
    private string _name = string.Empty;
    private LinkedObjectType _type;

    private LinkedObjectModelBuilder()
    {
    }

    public static LinkedObjectModelBuilder FromRelation(RelationResponse response)
    {
        return new LinkedObjectModelBuilder
        {
            _id = response.Id,
            _name = "Relatie: " + response.FirstName + " " + response.LastName,
        };
    }

    public static LinkedObjectModelBuilder FromPolicy(InsurancePolicyResponse response)
    {
        return new LinkedObjectModelBuilder
        {
            _id = response.Id,
            _name = "Polis: " + response.Type
        };
    }

    public static LinkedObjectModelBuilder FromDamageClaim(DamageClaimResponse response)
    {
        return new LinkedObjectModelBuilder
        {
            _id = response.Id,
            _name = "Schade: " + response.Type
        };
    }

    public LinkedObjectModelBuilder WithType(LinkedObjectType type)
    {
        _type = type;
        return this;
    }

    public LinkedObjectItem Build()
    {
        return new LinkedObjectItem(
            _id,
            _type,
            _name
        );
    }
}