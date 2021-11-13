using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace EShop.Web.TagHelpers;

[HtmlTargetElement("GoogleRecaptchaHelper")]
public class GoogleRecaptchaTagHelper : TagHelper
{
    private readonly IConfiguration _configuration;

    [HtmlAttributeName("asp-id")]
    public string Id { get; set; }

    [HtmlAttributeName("asp-callback")]
    public string Callback { get; set; }
    public GoogleRecaptchaTagHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var clientKeyValue = _configuration["GoogleRecaptcha:ClientKey"];
        if (context is null)
            throw new ArgumentNullException(nameof(context));
        if (context is null)
            throw new ArgumentNullException(nameof(output));
        if (Id is null)
            throw new ArgumentNullException(nameof(Id));
        if (Callback is null)
            throw new ArgumentNullException(nameof(Callback));

        output.TagName = "";
        output.TagMode = TagMode.StartTagAndEndTag;

        var loadingTagBuilder = new TagBuilder("img");
        loadingTagBuilder.MergeAttribute("width", "40");
        loadingTagBuilder.MergeAttribute("src", "/images/application/loading.gif");
        loadingTagBuilder.AddCssClass("captcha-loading");
        loadingTagBuilder.MergeAttribute("alt", "لطفا صبر کنید...");

        var captchaTagBuilder = new TagBuilder("div");
        captchaTagBuilder.MergeAttribute("data-sitekey", clientKeyValue);
        captchaTagBuilder.MergeAttribute("data-callback", Callback);
        captchaTagBuilder.GenerateId(Id, string.Empty);
        captchaTagBuilder.AddCssClass("g-recaptcha");

        var spanTagBuilder = new TagBuilder("span");
        spanTagBuilder.MergeAttribute("class", "text-danger");
        spanTagBuilder.MergeAttribute("data-valmsg-for", "GoogleReCaptchaResponse");

        output.Content.AppendHtml(loadingTagBuilder);
        output.Content.AppendHtml(captchaTagBuilder);
        output.Content.AppendHtml(spanTagBuilder);
    }
}
