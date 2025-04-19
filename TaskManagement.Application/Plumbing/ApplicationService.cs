using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.MappingProfiles;
using TaskManagement.Application.Services;
using TaskManagement.DTO.Office;
using TaskManagement.DTO.Office.Relation;
using TaskManagement.DTO.Office.Relation.DamageClaim;
using TaskManagement.DTO.Office.Relation.DamageClaim.Validators;
using TaskManagement.DTO.Office.Relation.InsurancePolicy;
using TaskManagement.DTO.Office.Relation.InsurancePolicy.Validators;
using TaskManagement.DTO.Office.Relation.Validators;
using TaskManagement.DTO.Office.User;
using TaskManagement.DTO.Office.User.Task;
using TaskManagement.DTO.Office.User.Task.LinkedObject;
using TaskManagement.DTO.Office.User.Task.LinkedObject.Validators;
using TaskManagement.DTO.Office.User.Task.Note;
using TaskManagement.DTO.Office.User.Task.Note.Validators;
using TaskManagement.DTO.Office.User.Task.Validators;
using TaskManagement.DTO.Office.User.Validators;
using TaskManagement.DTO.Office.Validators;

namespace TaskManagement.Application.Plumbing;

public static class ApplicationService
{
    public static IServiceCollection ConfigureAppDefaults(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(UserMappingProfile));
        services.AddAutoMapper(typeof(OfficeMappingProfile));
        services.AddAutoMapper(typeof(UserTaskMappingProfile));
        services.AddAutoMapper(typeof(NoteMappingProfile));
        services.AddAutoMapper(typeof(RelationMappingProfile));
        services.AddAutoMapper(typeof(DamageClaimMappingProfile));
        services.AddAutoMapper(typeof(InsurancePolicyMappingProfile));
        services.AddAutoMapper(typeof(LinkedObjectMappingProfile));

        services.AddScoped<IValidator<UserCreateDto>, UserCreateDtoValidator>();
        services.AddScoped<IValidator<UserUpdateDto>, UserUpdateDtoValidator>();

        services.AddScoped<IValidator<OfficeCreateDto>, OfficeCreateDtoValidator>();
        services.AddScoped<IValidator<OfficeUpdateDto>, OfficeUpdateDtoValidator>();

        services.AddScoped<IValidator<UserTaskCreateDto>, UserTaskCreateDtoValidator>();
        services.AddScoped<IValidator<UserTaskUpdateDto>, UserTaskUpdateDtoValidator>();

        services.AddScoped<IValidator<NoteCreateDto>, NoteCreateDtoValidator>();
        services.AddScoped<IValidator<NoteUpdateDto>, NoteUpdateDtoValidator>();

        services.AddScoped<IValidator<RelationCreateDto>, RelationCreateDtoValidator>();
        services.AddScoped<IValidator<RelationUpdateDto>, RelationUpdateDtoValidator>();

        services.AddScoped<IValidator<DamageClaimCreateDto>, DamageClaimCreateDtoValidator>();
        services.AddScoped<IValidator<DamageClaimUpdateDto>, DamageClaimUpdateDtoValidator>();

        services.AddScoped<IValidator<InsurancePolicyCreateDto>, InsurancePolicyCreateDtoValidator>();
        services.AddScoped<IValidator<InsurancePolicyUpdateDto>, InsurancePolicyUpdateDtoValidator>();

        services.AddScoped<IValidator<LinkedObjectCreateDto>, LinkedObjectCreateDtoValidator>();
        services.AddScoped<IValidator<LinkedObjectUpdateDto>, LinkedObjectUpdateDtoValidator>();

        services.AddScoped<OfficeService>();
        services.AddScoped<UserService>();
        services.AddScoped<UserTaskService>();
        services.AddScoped<NoteService>();
        services.AddScoped<RelationService>();
        services.AddScoped<DamageClaimService>();
        services.AddScoped<InsurancePolicyService>();

        return services;
    }
}