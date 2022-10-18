using Shopping.Migrations;
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
        public string ImageFullPath => ImageSource == String.Empty
            ? $"https://localhost:7288/images/noimage.png"
            : ImageSource;


        public string ImageSource { get; set; }

    }
}
