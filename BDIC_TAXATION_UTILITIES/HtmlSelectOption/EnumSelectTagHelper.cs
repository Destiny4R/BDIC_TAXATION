using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDIC_TAXATION_UTILITIES.HtmlSelectOption
{
    [HtmlTargetElement("enum-select", Attributes = "asp-enum-type")]
    public class EnumSelectTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-enum-type")]
        public Type EnumType { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select";

            var items = Enum.GetValues(EnumType)
                .Cast<Enum>()
                .Select(e => new SelectListItem
                {
                    Value = Convert.ToInt32(e).ToString(), Text = e.GetType().GetMember(e.ToString())[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is DescriptionAttribute attr  ? attr.Description : e.ToString()
                });
                output.Content.AppendHtml(string.Join("", items.Select(i => $"<option value='{i.Value}'>{i.Text}</option>")));
        }
    }
}
