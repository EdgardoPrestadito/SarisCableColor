using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Gerencias
{
    public class GerenciasViewModel
    {
        [Display(Name = "ID")]
        [Required]
        public int fiIDGerencia { get; set; }

        [Display(Name = "Usuario Gerencial")]
        [Required]
        public int fiIDUsuarioResponsable { get; set; }


        [Display(Name = "Descripción")]
        [Required]
        public string fcNombreGenerencia { get; set; }


        [Display(Name = "Usuario Gerencial")]
        [Required]
        public string fcNombreCorto { get; set; }


        [Display(Name = "Token")]
        public string fcToken { get; set; }

        [Display(Name = "Activo")]

        public bool fiEstado { get; set; }

        public bool EsEditar { get; set; }
    }
}