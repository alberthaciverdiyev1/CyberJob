using Microsoft.AspNetCore.Mvc;

namespace CyberJob.Controllers;
[Route("vacancies")]
public class VacancyController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}