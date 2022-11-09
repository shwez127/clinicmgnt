using Microsoft.AspNetCore.Mvc;

namespace Clinic_MVC_UI.Controllers
{
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
