using net_il_mio_fotoalbum.Models.Database_Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public List<Category>? Categories { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [MaxLength(500)]
        public string ImageUrl { get; set; }
        public byte[]? ImageFile {  get; set; }
        public string ImageSrc => ImageFile is null ? (ImageUrl is null ? "" : ImageUrl) : $"data:image/png;base64,{Convert.ToBase64String(ImageFile)}";
        public bool Visible { get; set; }

        public Photo() { }
    }
}
