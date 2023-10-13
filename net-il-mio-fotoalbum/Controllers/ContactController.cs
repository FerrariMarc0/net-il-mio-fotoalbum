using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Models.Database_Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(ContactMessage message)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", new { success = true });
            }

            return View("Index", message);
        }
    }
}
