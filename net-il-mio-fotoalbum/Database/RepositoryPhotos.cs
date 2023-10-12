using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models;
using net_il_mio_fotoalbum.Models.Database_Models;

namespace net_il_mio_fotoalbum.Database
{
    public class RepositoryPhotos : IRepositoryPhotos
    {
        private PhotoContext _db;
        public RepositoryPhotos(PhotoContext db)
        {
            _db = db;
        }

        public Photo GetPhotoById(int id)
        {
            Photo? photo = _db.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();

            if (photo != null)
            {
                return photo;
            }
            else
            {
                throw new Exception("La foto non è stata trovata");
            }
        }

        public List<Photo> GetPhotos()
        {
            List<Photo> photos = _db.Photos.Include(photo => photo.Categories).ToList();
            return photos;
        }

        public List<Photo> GetPhotoByTitle(string title)
        {
            List<Photo> foundedPhotos = _db.Photos.Where(photo => photo.Title.ToLower().Contains(title.ToLower())).ToList();
            return foundedPhotos;
        }

        public bool AddPhoto(Photo photoToAdd)
        {
            try
            {
                _db.Photos.Add(photoToAdd);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
            

            
        }

        public bool EditPhoto(int id, Photo updatedPhoto)
        {
            Photo? photoToUpdate = _db.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();

            if (photoToUpdate != null)
            {
                photoToUpdate.Categories.Clear();

                photoToUpdate.Title = updatedPhoto.Title;
                photoToUpdate.Description = updatedPhoto.Description;
                photoToUpdate.ImageUrl = updatedPhoto.ImageUrl;

                foreach (Category category in updatedPhoto.Categories)
                {
                    photoToUpdate.Categories.Add(category);
                }

                _db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeletePhoto(int id)
        {
            Photo? photoToDelete = _db.Photos.Where(photo => photo.Id == id).FirstOrDefault();

            if (photoToDelete == null)
            {
                return false;
            }

            _db.Photos.Remove(photoToDelete);
            _db.SaveChanges();

            return true;
        }
    }
}
