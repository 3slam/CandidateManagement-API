using CandidateManagement.Application.Requests;
using System.Text.RegularExpressions;

namespace CandidateManagement.Application.Validators;

internal static class SkillValidator
{
    private static readonly Regex NoNumbersRegex = new Regex(@"^\D+$", RegexOptions.Compiled);
    public static bool IsValidSkill(AddSkillRequest addSkillDto, decimal yearsOfExperience)
    {
        return IsValidSkillName(addSkillDto.Name) && IsValidGainDate(addSkillDto.GainDate, yearsOfExperience);
    }

    private static bool IsValidSkillName(string skillName)
    {
        if (string.IsNullOrWhiteSpace(skillName))
            return false;

        return NoNumbersRegex.IsMatch(skillName.Trim());
    }

    private static bool IsValidGainDate(DateOnly gainDate, decimal yearsOfExperience)
    {
        if (yearsOfExperience < 0)
            return false;

        var totalMonths = (int)Math.Floor(yearsOfExperience * 12);
        var maxAllowedDate = DateOnly.FromDateTime(DateTime.Now).AddMonths(-totalMonths);

        return gainDate <= maxAllowedDate;
    }

    
}
