// Copyright Information
// ==================================
// AutoLot - AutoLot.Mvc - CarsController.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2026/09/06
// ==================================

namespace AutoLot.Mvc.Controllers;

public class CarsController(
    IAppLogger appLogger,
    ICarRepo carRepo,
    IMakeRepo makeRepo) : BaseCrudController<Car>(
    appLogger,
    carRepo)
{
    protected override SelectList GetLookupValues() =>
        new SelectList(
            makeRepo.GetAllAsList()
                .OrderBy(m => m.Name),
            nameof(Make.Id),
            nameof(Make.Name));

    [HttpGet("{makeId}/{makeName}")]
    public IActionResult ByMake(
        int makeId,
        string makeName)
    {
        ViewBag.MakeName = makeName;
        return View(carRepo.GetAllByAsList(makeId));
    }
    //public IActionResult BadEndPoint() => new OkObjectResult(5);
}