using CandidateManagement.Application.Contracts;
using CandidateManagement.Application.Services;
using CandidateManagement.Application.Validators;
using CandidateManagement.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;


namespace CandidateManagement.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidator<Candidate>, CandidateValidator>();  
        services.AddScoped<ICandidateService, CandidateService>();
        services.AddScoped<ICandidateSkillsService, CandidateSkillsService>();
        return services;
    }
}
