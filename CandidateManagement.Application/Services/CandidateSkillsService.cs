using CandidateManagement.Application.Contracts;
using CandidateManagement.Application.Repositories;
using CandidateManagement.Application.Requests;
using CandidateManagement.Application.Validators;
using CandidateManagement.Common.Models;
using CandidateManagement.Common.ResultPattern;
using CandidateManagement.Domain.Entities;


namespace CandidateManagement.Application.Services;

internal class CandidateSkillsService(ICandidateRepository repository) : ICandidateSkillsService
{
    public async Task<Result<Candidate>> AddSkillAsync(AddSkillRequest request, int candidateId)
    {
        var candidateFromDb = await repository.GetByIdAsync(candidateId);
        if (candidateFromDb is null)
            return Error.NotFound("Candidate not found.");

        if (candidateFromDb.Skills.Count >= candidateFromDb.MaxNumSkills)
            return Error.Conflict("Candidate already has the maximum number of skills.");

        if (!SkillValidator.IsValidSkill(request, candidateFromDb.YearsOfExperience))
            return Error.Validation("Skill is not valid.");

        var skill = new Skill
        {
            GainDate = request.GainDate,
            Name = request.Name,
            CandidateId = candidateId
        };

        candidateFromDb.Skills.Add(skill);
        await repository.SaveChangesAsync();

        return candidateFromDb;
    }
}