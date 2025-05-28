using TaskManagement.DTO.Office.Relation;
using TaskManagement.DTO.Office.Relation.InsurancePolicy;
using TaskManagement.DTO.Office.Relation.DamageClaim;
using TaskManagement.MobileApp.Models;

namespace TaskManagement.MobileApp.Services.Helpers.Builders;

public class LinkedObjectModelBuilder
{
    private string _name = string.Empty;
    private LinkedObjectType _type;

    private LinkedObjectModelBuilder()
    {
    }

    public static LinkedObjectModelBuilder FromRelation(RelationResponse response)
    {
        return new LinkedObjectModelBuilder
        {
            _name = "Relatie: " + response.FirstName + " " + response.LastName,
        };
    }

    public static LinkedObjectModelBuilder FromPolicy(InsurancePolicyResponse response)
    {
        return new LinkedObjectModelBuilder
        {
            _name = "Polis: " + response.Type
        };
    }

    public static LinkedObjectModelBuilder FromDamageClaim(DamageClaimResponse response)
    {
        return new LinkedObjectModelBuilder
        {
            _name = "Schade: " + response.Type
        };
    }

    public LinkedObjectModelBuilder WithType(LinkedObjectType type)
    {
        _type = type;
        return this;
    }

    public LinkedObjectModel Build()
    {
        return new LinkedObjectModel(
            _type,
            _name
        );
    }
}