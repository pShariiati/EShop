using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace EShop.Common.Mvc;

public class GoogleReCaptchaValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var errorResult = new Lazy<ValidationResult>(() =>
            new ValidationResult("اعتبار سنجی کپچا با خطا مواجه نشد.",
                new string[]
                {
                        validationContext.MemberName
                }));
        var error = new ValidationResult(ErrorMessage);

        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return errorResult.Value;
        }

        var configuration = (IConfiguration)validationContext.GetService(typeof(IConfiguration));
        var reCaptchaResponse = value.ToString();
        var reCaptchaSecret = configuration?.GetSection("GoogleRecaptcha:SecretKey").Value ?? "";


        using var httpClient = new HttpClient();
        var httpResponse = httpClient.GetAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={reCaptchaSecret}&response={reCaptchaResponse}")
            .Result;
        if (httpResponse.StatusCode != HttpStatusCode.OK)
        {
            return errorResult.Value;
        }

        var jsonResponse = httpResponse.Content.ReadAsStringAsync().Result;
        dynamic jsonData = JObject.Parse(jsonResponse);
        if (jsonData.success != true.ToString().ToLower())
        {
            return errorResult.Value;
        }
        return ValidationResult.Success;
    }
}
