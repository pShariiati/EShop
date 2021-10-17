using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EShop.Common.Attributes
{
    //[AttributeUsage(AttributeTargets.Property)]
    public class FileRequiredAttribute : BaseValidationAttribute, IClientModelValidator
    {
        private readonly string _errorMessage;

        public FileRequiredAttribute(string displayName)
        {
            _errorMessage = $"لطفا {displayName} را وارد نمایید";
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            //var propertyDisplayName = validationContext.DisplayName;
            var file = value as IFormFile;
            if (file == null || file.Length == 0)
            {
                return new ValidationResult(_errorMessage);
            }
            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            //var propertyDisplayName = context.ModelMetadata.DisplayName;
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-fileRequired", _errorMessage);
        }
        //public bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        //{
        //    if (attributes.ContainsKey(key))
        //    {
        //        return false;
        //    }
        //    attributes.Add(key, value);
        //    return true;
        //}
    }
}
