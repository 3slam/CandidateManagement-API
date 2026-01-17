namespace CandidateManagement.Application.Requests;

public class AddCandidateRequest
{
    public string Name { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public decimal YearsOfExperience { get; set; }
    public int? MaxNumSkills { get; set; }
 }
