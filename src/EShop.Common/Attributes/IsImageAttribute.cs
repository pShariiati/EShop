using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EShop.Common.Attributes
{
    //[AttributeUsage(AttributeTargets.Property)]
    public class IsImageAttribute : BaseValidationAttribute, IClientModelValidator
    {
        private readonly string[] _allowExtensions = new []
        {
            "image/png",
            "image/jpeg",
            "image/bmp",
            "image/gif"
        };
        private readonly string _errorMessage;

        public IsImageAttribute(string displayName)
        {
            _errorMessage = $"{displayName} حتما باید عکس باشد";
        }

        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            //var propertyDisplayName = validationContext.DisplayName;
            var file = value as IFormFile;
            if (file != null && file.Length > 0)
            {
                if (!_allowExtensions.Contains(file.ContentType))
                {
                    return new ValidationResult(_errorMessage);
                }
                try
                {
                    var img = Image.FromStream(file.OpenReadStream());
                }
                catch
                {
                    return new ValidationResult(_errorMessage);
                }
            }
            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            //var propertyDisplayName = context.ModelMetadata.DisplayName;
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-isImage", _errorMessage);
            MergeAttribute(context.Attributes, "data-val-whitelistextensions",
                string.Join(",", _allowExtensions));
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
