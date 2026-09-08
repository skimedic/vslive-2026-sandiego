// Copyright Information
// ==================================
// AutoLot - AutoLot.Mvc - CarsController.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2026/09/06
// ==================================

namespace AutoLot.Mvc.Controllers;

[Route("[controller]/[action]")]
public class CarsController : Controller
{
    [Route("/[controller]")]
    [Route("/[controller]/[action]")]
    [HttpGet]
    public IActionResult Index() => View();

    [HttpGet("{makeId}/{makeName}")]
    public IActionResult ByMake(
        int makeId,
        string makeName) =>
        View();

    [HttpGet("{id?}")]
    public IActionResult Details(
        int? id) =>
        View();

    [HttpGet]
    public IActionResult Create() => View();

    [HttpGet]
    public IActionResult Edit(
        int? id) =>
        View();

    [HttpGet]
    public IActionResult Delete(
        int? id) =>
        View();
}