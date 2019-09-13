using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PSSR.Common;
using System;
using System.Linq;

namespace PSSR.UI.Helpers.TagHelpers
{
    [HtmlTargetElement("div", Attributes = ForAttributeName)]
    public class ActivityStatusTagHelper : TagHelper
    {
        private const string ForAttributeName = "p-status";
        private IUrlHelperFactory urlHelperFactory;
        public ActivityStatusTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        public ActivityStatus PStatus { set; get; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            TagBuilder tag = new TagBuilder("p");
            switch(PStatus)
            {
                case ActivityStatus.NotStarted:
                    tag.AddCssClass("text-orange");
                    break;
                case ActivityStatus.Done:
                    tag.AddCssClass("text-green");
                    break;
                case ActivityStatus.Ongoing:
                    tag.AddCssClass("text-blue");
                    break;
                case ActivityStatus.Reject:
                    tag.AddCssClass("text-red");
                    break;
                case ActivityStatus.Delete:
                    tag.AddCssClass("text-red");
                    break;
            }
            tag.InnerHtml.Append(PStatus.ToString());
            output.Content.AppendHtml(tag);
        }
    }
}
