using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers.API
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ApiPhotoController : ControllerBase
    {
        private PhotoContext _myDb;
        public ApiPhotoController(PhotoContext myDb)
        {
            _myDb = myDb;
        }

        [HttpGet]
        public IActionResult GetPhotos()
        {
            
            List<Photo> photos = _myDb.Photos.Include(photo => photo.Categories).ToList();

            return Ok(photos);
            
        }

        [HttpGet]
        public IActionResult SearchPhotos(string? search)
        {
            if(search == null)
            {
                return BadRequest(new { Message = "Non hai inserito nessuna stringa di ricerca." });
            }

            List<Photo> foundedPhotos = _myDb.Photos.Where(photo => photo.Title.ToLower().Contains(search.ToLower())).ToList();

            return Ok(foundedPhotos);
            
        }

        [HttpGet("{id}")]
        public IActionResult PhotoById(int id)
        {
            Photo? photo = _myDb.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();

            if(photo != null)
            {
                return Ok(photo);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] Photo newPhoto)
        {
            try
            {
                _myDb.Photos.Add(newPhoto);
                _myDb.SaveChanges();

                return Ok();

            }catch(Exception ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Photo updatedPhoto)
        {
            Photo? photoToUpdate = _myDb.Photos.Where(photo => photo.Id == id).FirstOrDefault();

            if(photoToUpdate == null)
            {
                return NotFound();
            }
            photoToUpdate.Title = updatedPhoto.Title;
            photoToUpdate.Description = updatedPhoto.Description;
            photoToUpdate.ImageUrl = updatedPhoto.ImageUrl;

            _myDb.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Photo? photoToDelete = _myDb.Photos.Where(photo => photo.Id == id).FirstOrDefault();

            if(photoToDelete == null)
            {
                return NotFound();
            }

            _myDb.Photos.Remove(photoToDelete);
            _myDb.SaveChanges();

            return Ok();
        }
    }
}
