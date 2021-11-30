using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Microsoft.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement("thuan", Attributes = DataValidationKeyAttributeName)]
    public class ValidationKeyTagHelper : TagHelper
    {
        private const string DataValidationKeyAttributeName = "data-valmsg-key";
        private const string ValidationKeyAttributeName = "asp-validation-key";
        /// <summary>
        /// Creates a new <see cref="ValidationMessageTagHelper"/>.
        /// </summary>
        /// <param name="generator">The <see cref="IHtmlGenerator"/>.</param>
        public ValidationKeyTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        /// <summary>
        /// Gets the <see cref="Rendering.ViewContext"/> of the executing view.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Gets the <see cref="IHtmlGenerator"/> used to generate the <see cref="ValidationMessageTagHelper"/>'s output.
        /// </summary>
        protected IHtmlGenerator Generator { get; }


        /// <summary>
        /// Gets an expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(ValidationKeyAttributeName)]
        public ModelExpression Key { get; set; }

        /// <inheritdoc />
        /// <remarks>Does nothing if <see cref="Key"/> is <c>null</c>.</remarks>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null) {
                throw new ArgumentNullException(nameof(output));
            }

            if (Key != null) {
                // Ensure Generator does not throw due to empty "fullName" if user provided data-valmsg-for attribute.
                // Assume data-valmsg-for value is non-empty if attribute is present at all. Should align with name of
                // another tag helper e.g. an <input/> and those tag helpers bind Name.
                IDictionary<string, object> htmlAttributes = null;
                if (string.IsNullOrEmpty(Key.Name) &&
                    string.IsNullOrEmpty(ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix) &&
                    output.Attributes.ContainsName(ValidationKeyAttributeName)) {
                    htmlAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { ValidationKeyAttributeName, "-non-empty-value-" },
                    };
                }

                string message = null;
                if (!output.IsContentModified) {
                    var tagHelperContent = await output.GetChildContentAsync();

                    // We check for whitespace to detect scenarios such as:
                    // <span validation-for="Name">
                    // </span>
                    if (!tagHelperContent.IsEmptyOrWhiteSpace) {
                        message = tagHelperContent.GetContent();
                    }
                }
                var tagBuilder = Generator.GenerateValidationMessage(
                    ViewContext,
                    Key.ModelExplorer,
                    Key.Name,
                    message: message,
                    tag: null,
                    htmlAttributes: htmlAttributes);

                if (tagBuilder != null) {
                    output.MergeAttributes(tagBuilder);

                    // Do not update the content if another tag helper targeting this element has already done so.
                    if (!output.IsContentModified && tagBuilder.HasInnerHtml) {
                        output.Content.SetHtmlContent(tagBuilder.InnerHtml);
                    }
                }
            }
        }

    }
}
