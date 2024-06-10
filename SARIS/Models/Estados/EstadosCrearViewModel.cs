using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Estados
{
    public class EstadosCrearViewModel
    {
        [Display(Name = "Descripción")]
        [Required]
        public string fcDescripcionEstado { get; set; }
        
        [Display(Name = "Clase")]
        [Required]
        public string fcClaseColor { get; set; }

        [Display(Name = "Id")]
        public int fiIDEstado { get; set; }

        public bool EsEditar { get; set; }

    }
}