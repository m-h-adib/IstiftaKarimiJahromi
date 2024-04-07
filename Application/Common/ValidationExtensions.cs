
using FluentValidation.Results;

namespace Application.Common;

public static class ValidationExtensions
{
    public static string ErrorMessage(this ValidationResult result)
    {
        var errors = result.Errors.Select(e => e.ErrorMessage);

        return string.Join("|", errors);
    }
}

