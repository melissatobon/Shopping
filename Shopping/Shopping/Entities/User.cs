using Microsoft.AspNetCore.Identity;
using Shopping.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Shopping.Entities
{
    public class User: IdentityUser
    {
        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [Display(Name = "Ciudad")]
        public City City { get; set; }


        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Address { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; } //GUID- Codigo numero alfanumerico


        //[Display(Name = "Foto")]
        //public string ImageFullPath => ImageId == Guid.Empty
        //    ? $"https://localhost:7288/images/noimage.png"
        //    : $"https://shoppingzulu.blob.core.windows.net/users/{ImageId}";
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageSource == String.Empty
            ? $"https://localhost:7288/images/noimage.png"
            : ImageSource;

        [Display(Name = "Tipo de usuario")]
        public UserType UserType { get; set; }


        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Usuario")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";
        public string ImageSource { get; set; }
        public string ImageName { get; set; }
        public ICollection<Sale> Sales { get; set; }
    }
}
