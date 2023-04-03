using System.ComponentModel.DataAnnotations;
using System.Globalization;

using Microsoft.AspNetCore.Http;

namespace CS_Utils.DataAnnotations;
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class FileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSizeInBytes;

    public FileSizeAttribute(int maxFileSizeInBytes)
        : base("The {0} field size cannot be bigger than {1} bytes")
    {
        _maxFileSizeInBytes = maxFileSizeInBytes;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile formFile && formFile.Length > _maxFileSizeInBytes)
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

        return ValidationResult.Success;
    }

    public override string FormatErrorMessage(string name)
        => string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, _maxFileSizeInBytes);
}
