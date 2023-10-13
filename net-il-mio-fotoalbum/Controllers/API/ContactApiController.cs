using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models.Database_Models;

namespace net_il_mio_fotoalbum.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactApiController : ControllerBase
    {
        private readonly PhotoContext _db;

        public ContactApiController(PhotoContext db)
        {
            _db = db;
        }

        [HttpPost("SaveMessage")]
        public IActionResult SaveMessage(ContactMessage message)
        {
            if (message != null)
            {
                _db.ContactMessages.Add(message);
                _db.SaveChanges();
                return Ok(new { message = "Messaggio salvato con successo." });
            }
            return BadRequest(new { message = "Errore nel salvataggio del messaggio." });
        }
    }
}
