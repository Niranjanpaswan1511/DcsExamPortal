using Microsoft.AspNetCore.Mvc;

namespace DcsExamPortal.Web.Controllers
{
    public class UserController : Controller
    {

        public IActionResult Dashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");

          
            if (string.IsNullOrEmpty(role) || role != "User")
            {
               
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }
        [HttpGet]
        public IActionResult FillForm(int id)
        {
            var role = HttpContext.Session.GetString("UserRole");

           
            if (string.IsNullOrEmpty(role) || role != "User")
            {
            
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.id = id;
            return View();
        }
        public IActionResult Receipt()
        {
            var role = HttpContext.Session.GetString("UserRole");

 
            if (string.IsNullOrEmpty(role) || role != "User")
            {
          
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }

        public IActionResult FormList()
        {
            return View();
        }
    }
}
