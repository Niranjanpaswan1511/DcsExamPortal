using Microsoft.AspNetCore.Mvc;

namespace DcsExamPortal.Web.Controllers
{
    public class AdminController : Controller
    {


        public IActionResult Dashboard()
        {

            var role = HttpContext.Session.GetString("UserRole");

     
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
               
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }
        public IActionResult Forms()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
               
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        public IActionResult Formlist()
        {
       
            var role = HttpContext.Session.GetString("UserRole");

           
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
             
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }
        [HttpGet]
        public IActionResult PreviewForm(int id)
        {
           
            var role = HttpContext.Session.GetString("UserRole");

           
            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
               
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.id = id;

            return View();
        }


        public IActionResult Submissions()
        {
            return View();
        }
        public IActionResult Payments()
        {
            return View();
        }


    }
}
