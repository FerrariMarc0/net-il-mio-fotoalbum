using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models.Database_Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? MessageText { get; set; }
    }
}
