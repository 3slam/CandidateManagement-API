namespace CandidateManagement.Domain.Entities;

public class Candidate
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Nickname { get; set; } = null!; 
    public string Email { get; set; } = null!; 
    public decimal YearsOfExperience { get; set; }
    public int? MaxNumSkills { get; set; }

    public virtual ICollection<Skill> Skills { get; set; } = new HashSet<Skill>();
}
