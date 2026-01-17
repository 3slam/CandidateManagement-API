using CandidateManagement.Domain.Entities;

namespace CandidateManagement.Application.Requests;

public class AddSkillRequest
{
    public string Name { get; set; } = null!;
    public DateOnly GainDate { get; set; }
}
