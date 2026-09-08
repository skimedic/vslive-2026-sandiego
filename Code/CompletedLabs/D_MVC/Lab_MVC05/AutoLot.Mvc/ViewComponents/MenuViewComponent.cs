// Copyright Information
// ==================================
// AutoLot - AutoLot.Mvc - MenuViewComponent.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2026/09/06
// ==================================

namespace AutoLot.Mvc.ViewComponents;

public class MenuViewComponent(
    IMakeRepo makeRepo) : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var makes =
            makeRepo.GetAllAsList() ??
            [
            ];
        return View(
            "MenuView",
            makes);
    }
    //public async Task<IViewComponentResult> InvokeAsync()
    //{
    //    return await Task.Run<IViewComponentResult>(() =>
    //    {
    //        var makes = makeRepo.GetAllAsList() ?? [];
    //        return View("MenuView", makes);
    //    });
    //}
}