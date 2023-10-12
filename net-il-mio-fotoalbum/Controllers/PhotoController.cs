using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {
        public IActionResult Index()
        {
            using (PhotoContext db = new PhotoContext())
            {
                List<Photo> photos = db.Photos.ToList<Photo>();

                return View("Admin", photos);
            }
        }

        public IActionResult Details(int id)
        {
            using (PhotoContext db = new PhotoContext())
            {
                Photo? foundedPhoto = db.Photos.Where(photo => photo.Id == id).FirstOrDefault();

                if (foundedPhoto == null)
                {
                    return NotFound($"La foto con id: {id} non è stata trovata.");
                }
                else
                {
                    return View("Details", foundedPhoto);
                }


            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public IActionResult Create(Photo newPhoto)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }

            using(PhotoContext db = new PhotoContext())
            {
                db.Photos.Add(newPhoto);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update()
        {
            return View("Update");
        }

       /* [HttpPost]
        public IActionResult Update()
        {
            return View("Update");
        }*/

        [HttpPost]
        public IActionResult Delete()
        {
            return View("Delete");
        }
    }
}
