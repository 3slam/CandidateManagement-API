using System.Text.RegularExpressions;

namespace CandidateManagement.Application.Validators;

internal static class EmailValidator
{
    private static readonly Regex EmailRegex = new Regex(
        @"^(?=.{1,254}$)(?=.{1,64}@)[A-Za-z0-9._%+\-]+@(?:[A-Za-z0-9](?:[A-Za-z0-9\-]{0,61}[A-Za-z0-9])?\.)+[A-Za-z]{2,}$", RegexOptions.Compiled  );

    public static bool IsValid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        email = email.Trim();
 
        if (email.Contains(' ') || email.Contains(".."))
            return false;

        return EmailRegex.IsMatch(email);
    }
}
