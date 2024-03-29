﻿namespace WebStore.API.ValidationClasses
{
    public class ValidationMessage
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(PropertyName))
            {
                return $"{ErrorMessage}";
            }
            else
            {
                return $"{ErrorMessage} ({PropertyName})";
            }
        }
    }
}
