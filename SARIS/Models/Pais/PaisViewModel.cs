using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OrionCoreCableColor.Models.Pais
{
    public class PaisViewModel
    {
        public int fiIDPais { get; set; }

        [Display(Name = "Pais")]
        [Required]
        public string fcPais { get; set; }
        public bool EsEditar { get; internal set; }
    }
}