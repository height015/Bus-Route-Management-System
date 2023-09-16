using System;
using System.ComponentModel.DataAnnotations;

namespace Extention;

public class EntityValidatorHelper
{
    public static bool Validate(object obj, out List<ValidationResult> results)
    {
        results = new List<ValidationResult>();
        ValidationContext validationContext = new ValidationContext(obj, null, null);
        return Validator.TryValidateObject(obj, validationContext, results, validateAllProperties: true);
    }
}

