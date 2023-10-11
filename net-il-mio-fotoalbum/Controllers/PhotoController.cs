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




    }
}
