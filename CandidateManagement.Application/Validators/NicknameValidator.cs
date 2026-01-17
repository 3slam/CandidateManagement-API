
using System.Text.RegularExpressions;

namespace CandidateManagement.Application.Validators;

internal static class NicknameValidator
{
 
    private static readonly Regex NicknameRegex = new Regex(@"\d[§®™©ʬ@]", RegexOptions.Compiled);

    public static bool IsValid(string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname))
            return false;

        return NicknameRegex.IsMatch(nickname);
    }
}
