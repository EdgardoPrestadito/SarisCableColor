using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrionCoreCableColor.Models.Areas
{
    public class AreasCrearViewModel
    {

        [Display(Name = "Id")]
        public int fiIDArea { get; set; }

        [Display(Name = "Nombre de Area")]
        [Required]
        public string fcDescripcion { get; set; }

        [Display(Name = "Correo Electronico")]
        [Required]
        public string fcCorreoElectronico { get; set; }

        [Display(Name = "Responsable de Area")]
        [Required]
        public int fiIDUsuarioResponsable { get; set; }

        [Display(Name = "Gerencia")]
        [Required]
        public int fiIDGerencia { get; set; }

        [Display(Name = "Estado")]

        public string fcNombreCorto { get; set; }

        public bool EsEditar { get; set; }


    }
}