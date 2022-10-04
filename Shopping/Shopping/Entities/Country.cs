using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Entities
{
    public class Country
    {
        public int Id { get; set; }
        [Display(Name= "Pais")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo 50 caracteres")]
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        public string Name { get; set; }
    }
}
