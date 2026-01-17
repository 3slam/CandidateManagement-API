using CandidateManagement.Application.Contracts;
using CandidateManagement.Application.Repositories;
using CandidateManagement.Application.Requests;
using CandidateManagement.Application.Validators;
using CandidateManagement.Common.Models;
using CandidateManagement.Common.ResultPattern;
using CandidateManagement.Domain.Entities;
using FluentValidation;
using System.Text;


namespace CandidateManagement.Application.Services;

internal class CandidateService(
    IValidator<Candidate> validator,
    ICandidateRepository repository) : ICandidateService
{
    public async Task<Result<IReadOnlyCollection<Candidate>>> UploadAsync(StreamReader reader)
    {
        try
        {
            var candidates = new List<Candidate>();
            var fileEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string? line;
            bool isHeader = true;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split("\t\t", StringSplitOptions.TrimEntries | StringSplitOptions.None );

                if (parts.Length < 4)
                    continue;

                var name = parts[0];
                var nickname = parts[1];
                var email = parts[2];

                if (!int.TryParse(parts[3], out var years))
                    continue;

                int? maxSkills = null;
                if (parts.Length > 4 && int.TryParse(parts[4], out var ms))
                    maxSkills = ms;

                if (!NicknameValidator.IsValid(nickname) ||
                    !EmailValidator.IsValid(email))
                    continue;

                if (!fileEmails.Add(email))
                    continue;

                candidates.Add(new Candidate
                {
                    Name = name,
                    Nickname = nickname,
                    Email = email.ToLowerInvariant(),
                    YearsOfExperience = years,
                    MaxNumSkills = maxSkills
                });
            }

            if (candidates.Count == 0)
                return Array.Empty<Candidate>();
 
            await repository.AddRangeAsync(candidates);
            await repository.SaveChangesAsync();
            return candidates;
        }
        catch (Exception ex)
        {
            return Error.Failure($"Upload failed: {ex.Message}");
        }
    }


    public async Task<Result<Stream>> ExportAsync()
    {
        var candidates = await repository.GetAllAsync();

        var sb = new StringBuilder();

        sb.AppendLine("#Name\t\t#NickName\t\t#Email\t\t#YearsOfExperience\t\t#MaxNumSkills");

        foreach (var c in candidates)
        {
            sb.Append(c.Name).Append("\t\t")
              .Append(c.Nickname).Append("\t\t")
              .Append(c.Email).Append("\t\t")
              .Append(c.YearsOfExperience.ToString()).Append("\t\t")
              .Append(c.MaxNumSkills?.ToString() ?? string.Empty)
              .AppendLine();
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        var stream = new MemoryStream(bytes);

        stream.Position = 0;
        return stream;
    }

    public async Task<Result<Candidate>> EditAsync(Candidate candidate)
    {
        var validation = await validator.ValidateAsync(candidate);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .Select(e => e.ErrorMessage)
                .ToArray();
            return Error.Validation(string.Join(", ", errors));
        }

        var candidateFromDb = await repository.GetByIdAsync(candidate.Id);
        if (candidateFromDb is null)
            return Error.NotFound("Candidate not found.");

      
        if (await repository.EmailExistsAsync(candidate.Email.Trim(), candidate.Id))
            return Error.Conflict("Email already exists.");
 
        candidateFromDb.Name = candidate.Name.Trim();
        candidateFromDb.Nickname = candidate.Nickname.Trim();
        candidateFromDb.Email = candidate.Email.ToLowerInvariant().Trim();
        candidateFromDb.YearsOfExperience = candidate.YearsOfExperience;
        candidateFromDb.MaxNumSkills = candidate.MaxNumSkills;
 
        await repository.SaveChangesAsync();

        return candidate;
    }

    public async Task<Result<Candidate>> AddAsync(AddCandidateRequest request)
    {
        var candidate = new Candidate
        {
            Name = request.Name?.Trim() ?? string.Empty,
            Nickname = request.Nickname?.Trim() ?? string.Empty,
            Email = request.Email.ToLowerInvariant().Trim() ?? string.Empty,
            YearsOfExperience = request.YearsOfExperience,
            MaxNumSkills = request.MaxNumSkills
        };

        var validation = await validator.ValidateAsync(candidate);
        if (!validation.IsValid)
        {
            var errors = validation.Errors
                .Select(e => e.ErrorMessage)
                .ToArray();
            return Error.Validation(string.Join(", ", errors));
        }

        if (await repository.EmailExistsAsync(candidate.Email.Trim()))
            return Error.Conflict("Email already exists.");

        await repository.AddAsync(candidate);

        await repository.SaveChangesAsync();
        return candidate;
    }
}
