using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P01_BillsPaymentSystem.Data.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class XorAttribute : ValidationAttribute
    {
        private string targetAttributeName;

        public XorAttribute(string targetAttribute)
        {
            this.targetAttributeName = targetAttribute;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentAttributeValue = value;
            var targetAttributeValue = validationContext.ObjectType
                                                   .GetProperty(this.targetAttributeName)
                                                   .GetValue(validationContext.ObjectInstance);

            if ((targetAttributeValue == null && currentAttributeValue == null) ||
                (targetAttributeValue != null && currentAttributeValue != null))
            {
                return new ValidationResult($"{this.targetAttributeName} and {validationContext.DisplayName} must have opposite values!");
            }
            return ValidationResult.Success;
        }
    }
}
