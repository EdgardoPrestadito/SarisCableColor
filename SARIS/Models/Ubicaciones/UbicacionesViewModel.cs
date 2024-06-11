using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Ubicaciones
{
    public class UbicacionesViewModel
    {
        [Key]
        public int fiUbicacion { get; set; }
        [Required]
        [Display(Name = "Nombre de Ubicación")]
        public string fcDescripcionUbicacion { get; set; }
        public string fcGeolocalización { get; set; }
        [Required]
        [Display(Name = "Descripción de Ubicación")]
        public string fcDescripciondeUbicacion { get; set; }
        public int fiActivo { get; set; }
        public bool EsEditar { get; set; }

    }
}