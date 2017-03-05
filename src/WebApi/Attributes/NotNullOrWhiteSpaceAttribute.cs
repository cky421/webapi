using System;

namespace WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class NotNullOrWhiteSpaceAttribute : Attribute
    {
        public void Validate(string value, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw  new ArgumentNullException(argumentName);
        }
    }
}