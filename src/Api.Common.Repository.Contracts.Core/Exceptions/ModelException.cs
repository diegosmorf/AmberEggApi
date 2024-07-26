using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Common.Repository.Exceptions
{
    public class ModelException : Exception
    {
        public ModelException(string message, IEnumerable<ValidationResult> errors) : base(message)
        {
            Errors = errors;
        }

        public ModelException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public IEnumerable<ValidationResult> Errors { get; protected set; }
    }
}