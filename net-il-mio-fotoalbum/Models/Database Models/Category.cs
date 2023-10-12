namespace net_il_mio_fotoalbum.Models.Database_Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Photo> Photos { get; set; }

        public Category() { }
    }
}
