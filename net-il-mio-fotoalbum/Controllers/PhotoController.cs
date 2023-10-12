using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SqlServer.Server;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using net_il_mio_fotoalbum.Models.Database_Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {
        private PhotoContext _myDb;

        public PhotoController(PhotoContext db)
        {
            _myDb = db;
        }

        public IActionResult Index()
        {
            List<Photo> photos = _myDb.Photos.ToList<Photo>();

            return View("Admin", photos);
            
        }

        public IActionResult Details(int id)
        {
            Photo? foundedPhoto = _myDb.Photos.Where(photo => photo.Id == id).FirstOrDefault();

            if (foundedPhoto == null)
            {
                return NotFound($"La foto con id: {id} non è stata trovata.");
            }
            else
            {
                return View("Details", foundedPhoto);
            }            
        }

        [HttpGet]
        public IActionResult Create()
        {
            List<SelectListItem> AllCategoriesSelectList = new List<SelectListItem>();
            List<Category> databaseAllCategory = _myDb.Categories.ToList();

            foreach(Category category in databaseAllCategory)
            {
                AllCategoriesSelectList.Add(new SelectListItem { Text = category.Title, Value = category.Id.ToString() });
            }

            PhotoFormModel model = new PhotoFormModel { Photo = new Photo(), Categories = AllCategoriesSelectList};

            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhotoFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<SelectListItem> allCategoriesSelectList = new List<SelectListItem>();
                List<Category> databaseAllCategories = _myDb.Categories.ToList();

                foreach (Category category in databaseAllCategories)
                {
                    allCategoriesSelectList.Add(new SelectListItem { Text = category.Title, Value = category.Id.ToString() });
                }

                data.Categories = allCategoriesSelectList;

                return View("Create", data);
            }

            data.Photo.Categories = new List<Category>();

            if(data.SelectedCategoriesId != null)
            {
                foreach(string categorySelectedId in data.SelectedCategoriesId)
                {
                    int intCategorySelectedId = int.Parse(categorySelectedId);

                    Category? categoryInDb = _myDb.Categories.Where(category => category.Id == intCategorySelectedId).FirstOrDefault();

                    if(categoryInDb != null) 
                    {
                        data.Photo.Categories.Add(categoryInDb);
                    }
                }
            }

            this.SetImageFileFromFormFile(data);

            _myDb.Photos.Add(data.Photo);
            _myDb.SaveChanges();
            

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

        private void SetImageFileFromFormFile(PhotoFormModel formData)
        {
            if(formData.ImageFormFile == null)
            {
                return;
            }

            MemoryStream stream = new MemoryStream();
            formData.ImageFormFile.CopyTo(stream);
            formData.Photo.ImageFile = stream.ToArray();
        }
    }
}
