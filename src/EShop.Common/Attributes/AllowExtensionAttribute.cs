using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace EShop.Common.Attributes;

//[AttributeUsage(AttributeTargets.Property)]
public class AllowExtensionsAttribute : BaseValidationAttribute, IClientModelValidator
{
    private readonly string[] _allowExtensions;
    private readonly string[] _allowContentTypes;
    private readonly string _errorMessage;

    public AllowExtensionsAttribute(string displayName, string[] allowExtensions, string[] allowContentTypes)
    {
        _allowExtensions = allowExtensions;
        _errorMessage = $"فرمت های مجاز برای {displayName} ";

        foreach (var allowExtension in allowExtensions)
        {
            _errorMessage += $"{allowExtension}, ";
        }

        _allowContentTypes = allowContentTypes;
        _errorMessage = _errorMessage.Trim(' ');
        _errorMessage = _errorMessage.Trim(',');
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
        }
        return ValidationResult.Success;
    }

    public void AddValidation(ClientModelValidationContext context)
    {
        //var propertyDisplayName = context.ModelMetadata.DisplayName;
        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-allowExtensions", _errorMessage);
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
