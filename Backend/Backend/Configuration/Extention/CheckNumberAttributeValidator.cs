using System;
using System.ComponentModel.DataAnnotations;
using Extention;

namespace Creative.Core.Extention;

[AttributeUsage(AttributeTargets.Property)]
public class CheckNumberAttribute : ValidationAttribute
{
        private readonly int _compareValue;

        public CheckNumberAttribute(int compareValue)
        {
            _compareValue = compareValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int)
            {
                int num = (int)value;
                if (num <= _compareValue)
                {
                    return new ValidationResult(base.ErrorMessageString);
                }

                return ValidationResult.Success;
            }

            if (value is long)
            {
                long num2 = (long)value;
                if (num2 <= _compareValue)
                {
                    return new ValidationResult(base.ErrorMessageString);
                }

                return ValidationResult.Success;
            }

            return new ValidationResult(base.ErrorMessageString);
        }
    }