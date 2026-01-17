using CandidateManagement.Domain.Entities;

namespace CandidateManagement.Application.Repositories;

public interface ICandidateRepository : IBaseRepository
{
    Task<IReadOnlyCollection<Candidate>> GetAllAsync();
    Task<Candidate?> GetByIdAsync(int id);
    Task<bool> EmailExistsAsync(string email, int? excludeId = null);
    Task<Candidate> AddAsync(Candidate candidate);
    Task<IEnumerable<string>> GetAllEmailsAsync();
    Task AddRangeAsync(List<Candidate> candidates);
}
