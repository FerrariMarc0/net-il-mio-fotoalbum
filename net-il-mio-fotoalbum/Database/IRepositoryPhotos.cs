using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Database
{
    public interface IRepositoryPhotos
    {
        public Photo GetPhotoById(int id);
        public List<Photo> GetPhotos();
        public List<Photo> GetPhotoByTitle(string title);
        public bool AddPhoto(Photo photoToAdd);
        public bool EditPhoto(int id, Photo updatedPhoto);
        public bool DeletePhoto(int id);
    }
}
