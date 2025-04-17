using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoColorado.Controllers
{
  [Authorize]
  public class HomeController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}