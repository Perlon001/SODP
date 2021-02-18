using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SODP.Domain.Model;
using SODP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SODP.UI.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PageInfo PageModel { get; set; }
        public string PageAction { get; set; }
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var tagBuilder = new TagBuilder("div");

            for(int i=1;i<=PageModel.TotalPages; i++)
            {
                var url = PageModel.Url.Replace(":", i.ToString());
                var tag = new TagBuilder("a");
                tag.Attributes["href"] = url;
                tag.AddCssClass(PageClass);
                tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                tag.InnerHtml.Append(i.ToString());
                tagBuilder.InnerHtml.AppendHtml(tag);
            }
            output.Content.AppendHtml(tagBuilder.InnerHtml);
        }
    }
}
