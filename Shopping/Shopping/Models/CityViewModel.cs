using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Shopping.Models
{
    public class CityViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Ciudad")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo 50 caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }
        public int StateId { get; set; }
    }
}
