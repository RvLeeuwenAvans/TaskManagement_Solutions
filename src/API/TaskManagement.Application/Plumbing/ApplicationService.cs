using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.MappingProfiles;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Office.User;
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
        
        //DI Auth First; then other services.
        services.AddScoped<TokenService>();

        services.AddScoped<IValidator<CreateUser>, UserCreateDtoValidator>();
        services.AddScoped<IValidator<UpdateUser>, UserUpdateDtoValidator>();

        services.AddScoped<IValidator<CreateOffice>, OfficeCreateDtoValidator>();
        services.AddScoped<IValidator<UpdateOffice>, OfficeUpdateDtoValidator>();

        services.AddScoped<IValidator<CreateUserTask>, UserTaskCreateDtoValidator>();
        services.AddScoped<IValidator<UpdateUserTask>, UserTaskUpdateDtoValidator>();

        services.AddScoped<IValidator<CreateNote>, NoteCreateDtoValidator>();
        services.AddScoped<IValidator<UpdateNote>, NoteUpdateDtoValidator>();

        services.AddScoped<IValidator<CreateRelation>, RelationCreateDtoValidator>();
        services.AddScoped<IValidator<UpdateRelation>, RelationUpdateDtoValidator>();

        services.AddScoped<IValidator<CreateDamageClaim>, CreateDamageClaimValidator>();
        services.AddScoped<IValidator<UpdateDamageClaim>, UpdateDamageClaimValidator>();

        services.AddScoped<IValidator<CreateInsurancePolicy>, CreateInsurancePolicyValidator>();
        services.AddScoped<IValidator<UpdateInsurancePolicy>, InsurancePolicyUpdateDtoValidator>();

        services.AddScoped<IValidator<CreateLinkedObject>, LinkedObjectCreateDtoValidator>();
        services.AddScoped<IValidator<UpdateLinkedObject>, LinkedObjectUpdateDtoValidator>();

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        
        services.AddScoped<OfficeService>();
        services.AddScoped<UserService>();
        services.AddScoped<UserTaskService>();
        services.AddScoped<NoteService>();
        services.AddScoped<RelationService>();
        services.AddScoped<DamageClaimService>();
        services.AddScoped<InsurancePolicyService>();
        services.AddScoped<LinkedObjectService>();

        return services;
    }
}