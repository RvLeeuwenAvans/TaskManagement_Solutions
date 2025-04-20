using AutoMapper;
using FluentValidation;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Office.Relation.DamageClaim;
using TaskManagement.DTO.Office.Relation.DamageClaim;

namespace TaskManagement.Application.Services
{
    public class DamageClaimService(
        IDamageClaimRepository damageClaimRepository,
        IMapper mapper,
        IValidator<DamageClaimCreateDto> createValidator,
        IValidator<DamageClaimUpdateDto> updateValidator)
    {
        public Task<List<DamageClaimResponseDto>> GetAllDamageClaimsAsync()
        {
            var damageClaims = damageClaimRepository.GetAll().ToList();
            var response = mapper.Map<List<DamageClaimResponseDto>>(damageClaims);

            return Task.FromResult(response);
        }

        public async Task<DamageClaimResponseDto?> GetDamageClaimByIdAsync(Guid id)
        {
            var damageClaim = await damageClaimRepository.GetByIdAsync(id);
            return damageClaim == null ? null : mapper.Map<DamageClaimResponseDto>(damageClaim);
        }

        public async Task<DamageClaimResponseDto> CreateDamageClaimAsync(DamageClaimCreateDto dto)
        {
            var validation = await createValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var damageClaim = mapper.Map<DamageClaim>(dto);
            await damageClaimRepository.AddAsync(damageClaim);
            return mapper.Map<DamageClaimResponseDto>(damageClaim);
        }

        public async Task<bool> UpdateDamageClaimAsync(DamageClaimUpdateDto dto)
        {
            var validation = await updateValidator.ValidateAsync(dto);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var damageClaim = await damageClaimRepository.GetByIdAsync(dto.Id);
            if (damageClaim == null) return false;

            mapper.Map(dto, damageClaim);
            await damageClaimRepository.UpdateAsync(damageClaim);
            return true;
        }

        public async Task<bool> DeleteDamageClaimAsync(Guid id)
        {
            var damageClaim = await damageClaimRepository.GetByIdAsync(id);
            if (damageClaim == null) return false;

            await damageClaimRepository.DeleteAsync(id);
            return true;
        }
    }
}