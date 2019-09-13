using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.TagHelpers
{
    [HtmlTargetElement("strong", Attributes = ForAttributeName)]
    public class AuthorTagHelper : TagHelper
    {
        private const string ForAttributeName = "p-author";
        private IUrlHelperFactory urlHelperFactory;
        public AuthorTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }
        public String PAuthor { set; get; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder tag = new TagBuilder("a");
            tag.Attributes.Add("href", "https://ir.linkedin.com/in/amin-sahranavard-b54392123");
            tag.InnerHtml.Append(PAuthor);
            output.Content.AppendHtml($"Copyright &copy; {DateTime.Now.Year.ToString()} ");
            output.Content.AppendHtml(tag);
            output.Attributes.Add("class", "author");
        }
    }
}
