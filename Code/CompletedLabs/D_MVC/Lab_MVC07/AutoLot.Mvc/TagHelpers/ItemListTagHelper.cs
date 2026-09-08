// Copyright Information
// ==================================
// AutoLot - AutoLot.Mvc - ItemListTagHelper.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2026/09/06
// ==================================

namespace AutoLot.Mvc.TagHelpers;

public class ItemListTagHelper : ItemLinkTagHelperBase
{
    public ItemListTagHelper(
        IHttpContextAccessor contextAccessor,
        IUrlHelperFactory urlHelperFactory) : base(
        contextAccessor,
        urlHelperFactory)
    {
        ActionName = nameof(CarsController.Index);
    }

    public override void Process(
        TagHelperContext context,
        TagHelperOutput output)
    {
        BuildContent(
            output,
            "text-default",
            "Back to List",
            "list");
    }
}