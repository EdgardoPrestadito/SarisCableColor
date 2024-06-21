using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.CI_Configuracion
{
    public class CI_ConfiguracionViewModel
    {
        [Display(Name = "Pais")]
        [Required]
        public Nullable<int> fiIDPais { get; set; }
        public string fcPais { get; set; }
        [Display(Name = "Region")]
        [Required]
        public Nullable<int> fiIDRegion { get; set; }
        public string fcRegion { get; set; }
        [Display(Name = "Ciudad")]
        [Required]
        public Nullable<int> fiIDCiudad { get; set; }
        public string fcCiudad { get; set; }
        [Display(Name = "Clasificacion CI")]
        [Required]
        public Nullable<int> fiIDCI { get; set; }
        public int fiIDConfiguracion { get; set; }
        [Display(Name = "CI Configuracion")]
        [Required]
        public string fcConfiguracion { get; set; }
        [Display(Name = "Latitud")]
        public string fcLatitud { get; set; }
        [Display(Name = "Longitud")]
        public string fcLongitud { get; set; }
        public string fcCI { get; set; }
        public bool EsEditar { get; set; }


    }
}