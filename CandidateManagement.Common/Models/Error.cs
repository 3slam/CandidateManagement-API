namespace CandidateManagement.Common.Models;

public record Error(string Description, ErrorType Type)
{
    public static readonly Error None = new(string.Empty, ErrorType.None);
    public static readonly Error NullValue = new("Null value was provided", ErrorType.Failure);
    public static Error Failure(string description) => new(description, ErrorType.Failure);
    public static Error NotFound(string description = "Not found") => new(description, ErrorType.NotFound);
    public static Error Conflict(string description) => new(description, ErrorType.Conflict);
    public static Error Authorization(string description) => new(description, ErrorType.Authorization);
    public static Error Validation(string description) => new(description, ErrorType.Validation);
}

