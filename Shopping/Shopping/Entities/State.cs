﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Shopping.Entities
{
    public class State
    {
        public int Id { get; set; }
        [Display(Name = "Departamento/Estado")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo 50 caracteres")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        public Country Country { get; set; }
        public ICollection<City> Cities { get; set; }
        [Display(Name = "Ciudades: ")]
        public int CitiesNumber => Cities == null ? 0 : Cities.Count;
    }
}
