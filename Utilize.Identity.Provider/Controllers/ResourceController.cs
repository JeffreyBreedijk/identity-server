using Microsoft.AspNetCore.Mvc;

namespace Utilize.Identity.Provider.Controllers
{
    public class ResourceController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return
            View();
        }
    }
}