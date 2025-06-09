using TaskManagement.DTO.Office.User.Task;
using TaskManagement.DTO.Office.User.Task.LinkedObject;
using TaskManagement.MobileApp.Helpers.Builders;
using TaskManagement.MobileApp.Models.Collections;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services;

public class LinkedObjectService(
    ILinkedObjectRepository linkedObjectRepository,
    IRelationRepository relationRepository,
    IPolicyRepository policyRepository,
    IDamageClaimRepository damageClaimRepository)
{
    public async Task<LinkedObjectItem?> GetLinkedObjectByResponse(LinkedObjectResponse linkedObjectLink)
    {
        return await (linkedObjectLink switch
        {
            { RelationId: { } relationId } when relationId != Guid.Empty => GetRelationModelAsync(relationId),
            { InsurancePolicyId: { } policyId } when policyId != Guid.Empty => GetPolicyModelAsync(policyId),
            { DamageClaimId: { } claimId } when claimId != Guid.Empty => GetDamageClaimModelAsync(claimId),
            _ => Task.FromResult<LinkedObjectItem?>(null)
        });

        async Task<LinkedObjectItem?> GetRelationModelAsync(Guid id)
        {
            var response = await relationRepository.GetRelationAsync(id);
            return LinkedObjectModelBuilder.FromRelation(response).WithType(LinkedObjectType.Relation).Build();
        }

        async Task<LinkedObjectItem?> GetPolicyModelAsync(Guid id)
        {
            var response = await policyRepository.GetInsurancePolicyAsync(id);
            return LinkedObjectModelBuilder.FromPolicy(response).WithType(LinkedObjectType.InsurancePolicy).Build();
        }

        async Task<LinkedObjectItem?> GetDamageClaimModelAsync(Guid id)
        {
            var response = await damageClaimRepository.GetDamageClaimAsync(id);
            return LinkedObjectModelBuilder.FromDamageClaim(response).WithType(LinkedObjectType.DamageClaim).Build();
        }
    }

    public async Task CreateLinkedObjectAsync(UserTaskResponse task, LinkedObjectItem linkedObject)
    {
        try
        {
            var dto = new CreateLinkedObject
            {
                UserTaskId = task.Id,
                RelationId = linkedObject.Type == LinkedObjectType.Relation ? linkedObject.Id : null,
                DamageClaimId = linkedObject.Type == LinkedObjectType.DamageClaim ? linkedObject.Id : null,
                InsurancePolicyId = linkedObject.Type == LinkedObjectType.InsurancePolicy ? linkedObject.Id : null
            };

            await linkedObjectRepository.CreateLinkedObjectAsync(dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}