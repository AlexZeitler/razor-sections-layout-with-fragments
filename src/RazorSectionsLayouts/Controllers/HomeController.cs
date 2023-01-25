using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RazorSectionsLayouts.Models;

namespace RazorSectionsLayouts.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;

  public List<Item> Items { get; set; } = new()
  {
    new(1, "Jane Doe"),
    new(2, "John Doe")
  };

  public HomeController(
    ILogger<HomeController> logger
  )
  {
    _logger = logger;
  }

  public IActionResult Index()
  {
    return View();
  }

  public IActionResult Privacy()
  {
    return View();
  }

  public IActionResult Parent()
  {
    return View(Items);
  }

  [ResponseCache(
    Duration = 0,
    Location = ResponseCacheLocation.None,
    NoStore = true
  )]
  public IActionResult Error()
  {
    return View(
      new ErrorViewModel
      {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
      }
    );
  }

  [HttpGet, Route("/childs/{id:int}")]
  public IActionResult ParentChild(
    [FromRoute] int id
  )
  {
    var item = Items.First(i => i.Id == id);
    return View(new Model(item, Items));
  }
}

public record Model(
  Item Item,
  List<Item> Items
);

public record Item(
  int Id,
  string Name
);
