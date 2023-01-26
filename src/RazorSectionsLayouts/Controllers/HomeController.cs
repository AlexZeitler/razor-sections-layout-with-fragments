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

  public IActionResult List()
  {
    return View("ParentChild", new ListModel(Items));
  }

  [HttpGet, Route("/childs/{id:int}")]
  public IActionResult Detail(
    [FromRoute] int id
  )
  {
    return View("ParentChild", new DetailsModel(Items, id));
  }
}

public class FragmentModel
{
  public FragmentModel(
    string fragmentId
  )
  {
    FragmentId = fragmentId;
  }

  public string FragmentId { get; set; }
}

// Item
public class ItemModel : FragmentModel
{
  public string Name { get; }
  public int Id { get; }
  public bool Selected { get; }

  public ItemModel(
    Item item,
    bool selected
  ) : base("list")
  {
    Selected = selected;
    Id = item.Id;
    Name = item.Name;
  }
}

// List only
public class ListModel : FragmentModel
{
  public List<ItemModel> Items { get; }

  public ListModel(
    List<Item> items,
    int selectedItemId = 0
  ) : base("list")
  {
    Items = items.Select(i => new ItemModel(i, i.Id == selectedItemId))
      .ToList();
  }
}

// List and Details
public class DetailsModel : FragmentModel
{
  public DetailsModel(
    List<Item> items,
    int selectedItemId
  ) : base("detail")
  {
    var selectedItem = items.FirstOrDefault(i => i.Id == selectedItemId) ?? throw new ArgumentNullException();
    SelectedItem = new ItemModel(selectedItem, true);
    List = new ListModel(items, selectedItemId);
  }

  public ItemModel SelectedItem { get; init; }
  public ListModel List { get; init; }
}

public record Item(
  int Id,
  string Name
);
