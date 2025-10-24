using Microsoft.AspNetCore.Mvc;

namespace DcsExamPortal.Web.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
