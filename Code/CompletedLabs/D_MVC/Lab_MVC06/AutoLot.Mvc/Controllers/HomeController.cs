// Copyright Information
// ==================================
// AutoLot - AutoLot.Mvc - HomeController.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2026/09/06
// ==================================

namespace AutoLot.Mvc.Controllers;

[Route("[controller]/[action]")]
public class HomeController(
    IAppLogger logger) : Controller
{
    [Route("/")]
    [Route("/[controller]")]
    [Route("/[controller]/[action]")]
    [HttpGet]
    public IActionResult Index(
        [FromServices]
        IOptionsSnapshot<DealerInfo> dealerOptionsSnapshot)
    {
        //logger.LogAppError("Test error");
        return View(dealerOptionsSnapshot.Value);
    }

    [HttpGet]
    public IActionResult GetServiceOne(
        [FromKeyedServices(nameof(SimpleServiceOne))]
        ISimpleService service)
    {
        return View(
            "SimpleService",
            service.SayHello());
    }

    [HttpGet]
    public IActionResult GetServiceTwo(
        [FromKeyedServices(nameof(SimpleServiceTwo))]
        ISimpleService service)
    {
        return View(
            "SimpleService",
            service.SayHello());
    }

    [HttpGet]
    public IActionResult GrantConsent()
    {
        HttpContext.Features
            .Get<ITrackingConsentFeature>()
            .GrantConsent();
        return RedirectToAction(
            nameof(Index),
            nameof(HomeController)
                .RemoveControllerSuffix(),
            new { area = "" });
    }

    [HttpGet]
    public IActionResult WithdrawConsent()
    {
        HttpContext.Features
            .Get<ITrackingConsentFeature>()
            .WithdrawConsent();
        return RedirectToAction(
            nameof(Index),
            nameof(HomeController)
                .RemoveControllerSuffix(),
            new { area = "" });
    }

    [HttpGet]
    public IActionResult RazorSyntax(
        [FromServices]
        ICarRepo carRepo)
    {
        var car = carRepo.Find(1);
        return View(car);
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(
        Duration = 0,
        Location = ResponseCacheLocation.None,
        NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}