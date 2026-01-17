using CandidateManagement.Application.Requests;
using CandidateManagement.Common.ResultPattern;
using CandidateManagement.Domain.Entities;


namespace CandidateManagement.Application.Contracts;

public interface ICandidateSkillsService
{
    Task<Result<Candidate>> AddSkillAsync(AddSkillRequest request, int candidateId);
}
