﻿using Microsoft.AspNetCore.Mvc.Rendering;
using net_il_mio_fotoalbum.Models.Database_Models;

namespace net_il_mio_fotoalbum.Models
{
    public class PhotoFormModel
    {
        public Photo Photo { get; set; }
        public IFormFile? ImageFormFile { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public List<string>? SelectedCategoriesId { get; set; }
    }
}
