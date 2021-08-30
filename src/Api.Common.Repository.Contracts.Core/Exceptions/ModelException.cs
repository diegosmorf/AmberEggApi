using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Api.Common.Repository.Exceptions
{
    [Serializable]
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

        protected ModelException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IEnumerable<ValidationResult> Errors { get; protected set; }
    }
}