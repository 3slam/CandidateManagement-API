namespace CandidateManagement.Application.Repositories;

public interface IBaseRepository
{
    Task<int> SaveChangesAsync();
}