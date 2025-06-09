using TaskManagement.DTO.Office.Relation;

namespace TaskManagement.MobileApp.Services.Repositories.Interfaces;

public interface IRelationRepository
{
    Task<RelationResponse> GetRelationAsync(Guid relationId);
    
    Task<List<RelationResponse>> GetRelationsByOfficeAsync(Guid officeId);
}