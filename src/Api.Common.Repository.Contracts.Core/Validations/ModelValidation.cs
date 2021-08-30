using Api.Common.Repository.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Api.Common.Repository.Validations
{
    public static class ModelValidation
    {
        public static IList<ValidationResult> ValidateModelAnnotations(this object instance)
        {
            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(instance);

            Validator.TryValidateObject(instance, validationContext, result, true);

            if (instance is IValidatableObject parsedInstance) parsedInstance.Validate(validationContext);

            return result;
        }

        public static void RaiseExceptionIfModelIsNotValid(this object instance)
        {
            var result = instance.ValidateModelAnnotations();

            if (result.Any())
                throw new ModelException(
                    "This object instance is not valid based on DataAnnotation definitions. See more details on Errors list.",
                    result);
        }
    }
}