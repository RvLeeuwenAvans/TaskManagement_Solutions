using TaskManagement.DTO.Office;
using TaskManagement.DTO.Office.User;
using TaskManagement.MobileApp.Helpers.Builders;
using TaskManagement.MobileApp.Models.Collections;
using TaskManagement.MobileApp.Models.Interfaces;
using TaskManagement.MobileApp.Services.Repositories.Interfaces;

namespace TaskManagement.MobileApp.Services;

public class OfficeService(
    IUserContext userContext,
    IUserRepository userRepository,
    IOfficeRepository officeRepository,
    IDamageClaimRepository damageClaimRepository,
    IPolicyRepository policyRepository,
    IRelationRepository relationRepository)
{
    public async Task<List<LinkedObjectItem>> GetManagedObjectsByOffice()
    {
        var relationsTask = relationRepository.GetRelationsByOfficeAsync(userContext.OfficeId);
        var policiesTask = policyRepository.GetInsurancePoliciesByOfficeAsync(userContext.OfficeId);
        var damageClaimsTask = damageClaimRepository.GetDamageClaimsByOfficeAsync(userContext.OfficeId);

        await Task.WhenAll(relationsTask, policiesTask, damageClaimsTask);

        var relations = await relationsTask;
        var linkedObjects = relations.Select(relation => LinkedObjectModelBuilder.FromRelation(relation)
                .WithType(LinkedObjectType.Relation)
                .Build())
            .ToList();

        var policies = await policiesTask;

        linkedObjects.AddRange(policies.Select(policy => LinkedObjectModelBuilder.FromPolicy(policy)
            .WithType(LinkedObjectType.InsurancePolicy)
            .Build()));

        var damageClaims = await damageClaimsTask;

        linkedObjects.AddRange(damageClaims.Select(damageClaim => LinkedObjectModelBuilder.FromDamageClaim(damageClaim)
            .WithType(LinkedObjectType.DamageClaim)
            .Build()));

        return linkedObjects;
    }

    public async Task<List<UserItem>> GetUsersByOfficeAsync()
    {
        var response = await userRepository.GetUsersByOfficeAsync(userContext.OfficeId);
        return response.Select(userResponse => UserItemBuilder.From(userResponse).Build()).ToList();
    }

    public async Task<UserItem> GetOfficeUserByIdAsync(Guid userId)
    {
        var response = await userRepository.GetUserById(userId);
        return UserItemBuilder.From(response).Build();
    }

    public async Task<OfficeResponse> GetCurrentUserOfficeAsync()
    {
        return await officeRepository.GetOfficeById(userContext.OfficeId);
    }
}