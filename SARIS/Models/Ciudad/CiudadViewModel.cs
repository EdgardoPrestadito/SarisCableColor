using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Ciudad
{
    public class CiudadViewModel
    {
        [Display(Name = "Pais")]
        [Required]
        public int fIIDPais { get; set; }
        public string fcPais { get; set; }
        [Display(Name = "Región")]
        [Required]
        public int fiIDRegion { get; set; }
        public string fcRegion { get; set; }
        [Display(Name = "Ciudad")]
        [Required]
        public int fiIDCiudad { get; set; }
        [Display(Name = "Ciudad")]
        [Required]
        public string fcCiudad { get; set; }
        public bool EsEditar { get; internal set; }
    }
}