using Microsoft.AspNetCore.Mvc;

namespace DevFitness.API.Controllers
{
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        [Route("/index")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
