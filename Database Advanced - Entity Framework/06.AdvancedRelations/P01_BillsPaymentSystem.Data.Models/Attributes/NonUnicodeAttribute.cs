using System;
using System.ComponentModel.DataAnnotations;

namespace P01_BillsPaymentSystem.Data.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NonUnicodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult($"Property <{validationContext.DisplayName}> cannot be null!");
            }

            var text = (string)value;
            foreach (var ch in text)
            {
                if (ch > 255)
                {
                    return new ValidationResult($"Property <{validationContext.DisplayName}> cannot contain non unicode characters!");
                }
            }

            return ValidationResult.Success;
        }
    }
}
