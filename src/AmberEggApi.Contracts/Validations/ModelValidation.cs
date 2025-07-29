using AmberEggApi.Contracts.Commands;
using AmberEggApi.Contracts.Exceptions;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AmberEggApi.Contracts.Validations;
public static class ModelValidation
{
    public static void Validate(this ICommand command)
    {
        var result = new List<ValidationResult>();
        var validationContext = new ValidationContext(command);

        Validator.TryValidateObject(command, validationContext, result, true);

        if (command is IValidatableObject parsedInstance)
            parsedInstance.Validate(validationContext);

        if (result.Any())
        {
            var errors = string.Join(", ", result.Select(x => x.ErrorMessage));
            throw new DomainModelException(
                $"This object instance is not valid based on DataAnnotation definitions. Details: {errors}");
        }
    }

    public static void Validate(this IUpdateCommand command)
    {
        Validate((ICommand)command);
        Validate(command.Id);
    }

    public static void Validate(this IDeleteCommand command)
    {
        Validate((ICommand)command);
        Validate(command.Id);
    }

    public static void Validate(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new DomainModelException("Id parameter is required");
        }
    }
}