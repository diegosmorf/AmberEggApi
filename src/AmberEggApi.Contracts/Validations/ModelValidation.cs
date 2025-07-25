using AmberEggApi.Contracts.Commands;
using AmberEggApi.Contracts.Exceptions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AmberEggApi.Contracts.Validations
{
    public static class ModelValidation
    {
        public static IList<ValidationResult> ValidateModelAnnotations(this ICommand instance)
        {
            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(instance);

            Validator.TryValidateObject(instance, validationContext, result, true);

            if (instance is IValidatableObject parsedInstance) parsedInstance.Validate(validationContext);

            return result;
        }

        public static void RaiseExceptionIfModelIsNotValid(this ICommand instance)
        {
            var result = instance.ValidateModelAnnotations();

            if (result.Any())
                throw new ModelException(
                    "This object instance is not valid based on DataAnnotation definitions. See more details on Errors list.",
                    result);
        }
    }
}