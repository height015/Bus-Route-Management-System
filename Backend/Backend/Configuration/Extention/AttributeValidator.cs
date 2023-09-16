using System;
using System.ComponentModel.DataAnnotations;
using Extention;

namespace Creative.Core.Extention;

[AttributeUsage(AttributeTargets.Property)]
public class CheckEmailAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string text = value as string;
        if (string.IsNullOrEmpty(text))
        {
            return ValidationResult.Success;
        }
        if (!text.IsEmailValid())
        {
            return new ValidationResult(base.ErrorMessageString);
        }
        return ValidationResult.Success;
    }
}

