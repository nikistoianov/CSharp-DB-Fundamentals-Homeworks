namespace Common.Validators
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AttributeValidator
    {
        public static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var result = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, result, true);

            return isValid;
        }

        public static bool IsValid(object obj, List<ValidationResult> result)
        {
            var validationContext = new ValidationContext(obj);
            var isValid = Validator.TryValidateObject(obj, validationContext, result, true);

            return isValid;
        }
    }
}
