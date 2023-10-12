using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            Photo? foundedPhoto = _myDb.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();

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
        public IActionResult Update(int id)
        {
            Photo? photoToEdit = _myDb.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();

            if (photoToEdit == null)
            {
                return NotFound("La pizza che vuoi modificare non è stata trovata");
            }
            else
            {
                List<Category> dbCategoriesList = _myDb.Categories.ToList();
                List<SelectListItem> selectListItems = new List<SelectListItem>();

                foreach (Category category in dbCategoriesList)
                {
                    selectListItems.Add(new SelectListItem { Value = category.Id.ToString(), Text = category.Title, Selected = photoToEdit.Categories.Any(categoryAssociated => categoryAssociated.Id == category.Id) });
                }

                PhotoFormModel model = new PhotoFormModel { Photo = photoToEdit, Categories = selectListItems };

                return View("Update", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PhotoFormModel data)
        {
            if (!ModelState.IsValid)
            {
                
                List<Category> dbIngredientList = _myDb.Categories.ToList();
                List<SelectListItem> selectListItems = new List<SelectListItem>();

                foreach (Category categories in dbIngredientList)
                {
                    selectListItems.Add(new SelectListItem { Value = categories.Id.ToString(), Text = categories.Title });
                }
                data.Categories = selectListItems;

                return View("Update", data);
            }


            Photo? photoToUpdate = _myDb.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();

            if (photoToUpdate != null)
            {
                photoToUpdate.Categories.Clear();

                photoToUpdate.Title = data.Photo.Title;
                photoToUpdate.Description = data.Photo.Description;
                photoToUpdate.ImageUrl = data.Photo.ImageUrl;

                if (data.SelectedCategoriesId != null)
                {
                    foreach (string categorySelectedId in data.SelectedCategoriesId)
                    {
                        int intCategorySelectedId = int.Parse(categorySelectedId);
                        Category? categoryInDb = _myDb.Categories.Where(category => category.Id == intCategorySelectedId).FirstOrDefault();

                        if (categoryInDb != null)
                        {
                            photoToUpdate.Categories.Add(categoryInDb);
                        }
                    }
                }

                _myDb.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("Mi dispiace, non è stata trovata la foto da aggiornare.");
            }
        }
    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Photo? photoToDelete = _myDb.Photos.Where(photo => photo.Id == id).FirstOrDefault();

            if (photoToDelete != null)
            {
                _myDb.Photos.Remove(photoToDelete);
                _myDb.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("La foto da eliminare non è stata trovata!");
            }
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
