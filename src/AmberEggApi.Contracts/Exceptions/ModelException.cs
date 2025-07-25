using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmberEggApi.Contracts.Exceptions;

public class ModelException(string message, IEnumerable<ValidationResult> errors) : Exception(message)
{
    public IEnumerable<ValidationResult> Errors { get; protected set; } = errors;
}