using CandidateManagement.Domain.Entities;
using FluentValidation;

namespace CandidateManagement.Application.Validators;

internal sealed class CandidateValidator : AbstractValidator<Candidate>
{
    public CandidateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Nickname)
            .NotEmpty()
            .Must(NicknameValidator.IsValid)
            .WithMessage("Nickname is not valid.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .Must(EmailValidator.IsValid)
            .WithMessage("Email is not valid.");

        RuleFor(x => x.YearsOfExperience).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaxNumSkills).GreaterThanOrEqualTo(0).When(x => x.MaxNumSkills.HasValue);
    }
}
