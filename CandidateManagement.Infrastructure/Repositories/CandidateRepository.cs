using CandidateManagement.Application.Repositories;
using CandidateManagement.Domain.Entities;
using CandidateManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CandidateManagement.Infrastructure.Repositories;

internal class CandidateRepository(ApplicationDbContext context) : ICandidateRepository
{
    public async Task<IReadOnlyCollection<Candidate>> GetAllAsync()
    {
        return await context.Candidates.AsNoTracking().Include(c => c.Skills).ToListAsync();
    }

    public async Task<Candidate?> GetByIdAsync(int id)
    {
        return await context.Candidates.Include(c => c.Skills).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await context.Candidates.AsNoTracking().AnyAsync(c => c.Email == email && c.Id != excludeId.Value);
        }
        return await context.Candidates.AsNoTracking().AnyAsync(c => c.Email == email);
    }

    public async Task<Candidate> AddAsync(Candidate candidate)
    {
        await context.Candidates.AddAsync(candidate);
        return candidate;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> GetAllEmailsAsync()
    {
       return await context.Candidates.AsNoTracking().Select(c => c.Email).ToListAsync();
    }

    public async Task AddRangeAsync(List<Candidate> candidates)
    {
        await context.Candidates.AddRangeAsync(candidates);
    }
}
