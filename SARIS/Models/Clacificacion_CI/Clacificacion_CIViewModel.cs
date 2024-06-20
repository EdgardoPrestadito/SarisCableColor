using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Clacificacion_CI
{
    public class Clacificacion_CIViewModel
    {
        [Display(Name = "Clasificación CI")]
        [Required]
        public int fiIDCI { get; set; }
        public string fcCI { get; set; }
        public bool EsEditar { get; internal set; }
    }
}