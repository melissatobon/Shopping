﻿using Shopping.Migrations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Shopping.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: Pending to change to the correct path
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
        ? $"https://localhost:7057/images/noimage.png"
        : $"https://shopping4.blob.core.windows.net/products/{ImageId}";


        //public string ImageSource { get; set; }

    }
}
