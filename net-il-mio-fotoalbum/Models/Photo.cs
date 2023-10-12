using net_il_mio_fotoalbum.Models.Database_Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public List<Category>? Categories { get; set; }
        [Required(ErrorMessage = "Il titolo è obbligatorio.")]
        [MaxLength(100, ErrorMessage = "La lunghezza massima è di 100 caratteri.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "La descrizione è obbligatoria.")]
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Link dell'immagine obbligatorio.")]
        [Url(ErrorMessage = "Inserisci un link immagine valido.")]
        [MaxLength(500, ErrorMessage = "Il link dell'immagine non deve superare i 500 caratteri.")]
        public string ImageUrl { get; set; }
        public byte[]? ImageFile {  get; set; }
        public string ImageSrc => ImageFile is null ? (ImageUrl is null ? "" : ImageUrl) : $"data:image/png;base64,{Convert.ToBase64String(ImageFile)}";
        public bool Visible { get; set; }

        public Photo() { }
    }
}
