// Copyright Information
// ==================================
// AutoLot - AutoLot.Mvc - ItemEditTagHelper.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2026/09/06
// ==================================

namespace AutoLot.Mvc.TagHelpers;

public class ItemEditTagHelper : ItemLinkTagHelperBase
{
    public ItemEditTagHelper(
        IHttpContextAccessor contextAccessor,
        IUrlHelperFactory urlHelperFactory) : base(
        contextAccessor,
        urlHelperFactory)
    {
        ActionName = nameof(CarsController.Edit);
    }

    public override void Process(
        TagHelperContext context,
        TagHelperOutput output)
    {
        BuildContent(
            output,
            "text-warning",
            "Edit",
            "edit");
    }
}