using CandidateManagement.Application.Requests;
using CandidateManagement.Common.ResultPattern;
using CandidateManagement.Domain.Entities;

namespace CandidateManagement.Application.Contracts;

public interface ICandidateService
{
    Task<Result<IReadOnlyCollection<Candidate>>> UploadAsync(StreamReader streamReader);
    Task<Result<Stream>> ExportAsync();
    Task<Result<Candidate>> EditAsync(Candidate candidate);
    Task<Result<Candidate>> AddAsync(AddCandidateRequest candidate);
}
