namespace CandidateManagement.Domain.Entities;

public class Skill
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly GainDate { get; set; }
    public virtual Candidate Candidate { get; set; } = null!;
}