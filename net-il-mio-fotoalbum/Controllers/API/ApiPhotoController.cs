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
        private IRepositoryPhotos _repoPhotos;
        public ApiPhotoController(IRepositoryPhotos repoPhotos)
        {
            _repoPhotos = repoPhotos;
        }

        [HttpGet]
        public IActionResult GetPhotos()
        {
            List<Photo> photos = _repoPhotos.GetPhotos();            
           
            return Ok(photos);
        }

        [HttpGet]
        public IActionResult SearchPhotos(string? search)
        {
            List<Photo> foundedPhotos = new List<Photo>();

            if(search == null)
            {
                foundedPhotos = _repoPhotos.GetPhotos();
            }
            else
            {
                foundedPhotos = _repoPhotos.GetPhotoByTitle(search);
            }

            return Ok(foundedPhotos);            
        }

        [HttpGet("{id}")]
        public IActionResult PhotoById(int id)
        {
            Photo photo = _repoPhotos.GetPhotoById(id);

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
               bool result = _repoPhotos.AddPhoto(newPhoto);

                if (result)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }

            }catch(Exception ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Photo updatedPhoto)
        {
            bool result = _repoPhotos.EditPhoto(id, updatedPhoto);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool result = _repoPhotos.DeletePhoto(id);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
