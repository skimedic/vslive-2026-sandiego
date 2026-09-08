// Copyright Information
// ==================================
// AutoLot - AutoLot.Mvc - BaseCrudController.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2026/09/06
// ==================================

namespace AutoLot.Mvc.Controllers.Base;

[Route("[controller]/[action]")]
public abstract class BaseCrudController<TEntity>(
    IAppLogger appLogger,
    IBaseRepo<TEntity> baseRepo) : Controller where TEntity : BaseEntity, new()
{
    protected readonly IAppLogger AppLoggerInstance = appLogger;
    protected readonly IBaseRepo<TEntity> BaseRepoInstance = baseRepo;
    protected virtual SelectList GetLookupValues() => null;

    protected TEntity GetOneEntity(
        int? id) =>
        id == null ? null : BaseRepoInstance.Find(id.Value);

    [Route("/[controller]")]
    [Route("/[controller]/[action]")]
    [HttpGet]
    public virtual IActionResult Index() => View(BaseRepoInstance.GetAllIgnoreQueryFiltersAsList());

    [HttpGet("{id?}")]
    public virtual IActionResult Details(
        int? id)
    {
        if (!id.HasValue)
        {
            return BadRequest();
        }

        var entity = GetOneEntity(id);
        return entity == null ? NotFound() : View(entity);
    }

    [HttpGet]
    public virtual IActionResult Create()
    {
        ViewData["LookupValues"] = GetLookupValues();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual IActionResult Create(
        TEntity entity)
    {
        if (!ModelState.IsValid)
        {
            ViewData["LookupValues"] = GetLookupValues();
            return View(entity);
        }

        BaseRepoInstance.Add(entity);
        return RedirectToAction(
            nameof(Details),
            new { id = entity.Id });
    }

    [HttpGet("{id?}")]
    public virtual IActionResult Edit(
        int? id)
    {
        if (!id.HasValue)
        {
            return BadRequest();
        }

        var entity = GetOneEntity(id);
        if (entity == null)
        {
            return NotFound();
        }

        ViewData["LookupValues"] = GetLookupValues();
        return View(entity);
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public virtual IActionResult Edit(
        int id,
        TEntity entity)
    {
        if (id != entity.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            ViewData["LookupValues"] = GetLookupValues();
            return View(entity);
        }

        BaseRepoInstance.Update(entity);
        return RedirectToAction(
            nameof(Details),
            new { id = entity.Id });
    }

    [HttpGet("{id?}")]
    public virtual IActionResult Delete(
        int? id)
    {
        if (!id.HasValue)
        {
            return BadRequest();
        }

        var entity = GetOneEntity(id);
        return entity == null ? NotFound() : View(entity);
    }

    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public virtual IActionResult Delete(
        int id,
        TEntity entity)
    {
        if (id != entity.Id)
        {
            return BadRequest();
        }

        BaseRepoInstance.Delete(entity);
        return RedirectToAction(nameof(Index));
    }
}