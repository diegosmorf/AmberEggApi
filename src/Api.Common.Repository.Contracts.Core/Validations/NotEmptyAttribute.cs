using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Common.Repository.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class NotEmptyAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must not be empty";
        public NotEmptyAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            //NotEmpty doesn't necessarily mean required
            if (value is null)
            {
                return true;
            }

            return value switch
            {
                Guid guid => guid != Guid.Empty,
                String text => !string.IsNullOrEmpty(text),
                int number => number != 0,
                _ => true,
            };
        }
    }
}